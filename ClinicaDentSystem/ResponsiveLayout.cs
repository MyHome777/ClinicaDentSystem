using System.Runtime.CompilerServices;

namespace ClinicaDentSystem
{
    internal static class ResponsiveLayout
    {
        private const float MinScale = 0.70f;
        private const float MaxScale = 1.15f;

        private sealed class LayoutState
        {
            public required Size DesignSize { get; init; }
            public required Dictionary<Control, ControlSnapshot> Snapshots { get; init; }
            public Size LastAppliedSize { get; set; }
        }

        private sealed class ControlSnapshot
        {
            public required Rectangle Bounds { get; init; }
            public required Padding Margin { get; init; }
            public required Padding Padding { get; init; }
            public required Font Font { get; init; }
            public required bool AutoSize { get; init; }
        }

        private static readonly ConditionalWeakTable<Control, LayoutState> States = new();

        public static void Configure(Control root)
        {
            if (root.IsDisposed || States.TryGetValue(root, out _))
            {
                return;
            }

            if (root is ContainerControl container)
            {
                container.AutoScaleMode = AutoScaleMode.Dpi;
            }

            if (root is UserControl userControl)
            {
                userControl.AutoScroll = true;
            }
            else if (root is Form form)
            {
                form.AutoScroll = true;
                form.StartPosition = FormStartPosition.CenterScreen;
            }

            var snapshots = new Dictionary<Control, ControlSnapshot>();
            CaptureSnapshots(root, snapshots);
            NormalizeControls(root);

            var state = new LayoutState
            {
                DesignSize = GetDesignSize(root),
                Snapshots = snapshots,
                LastAppliedSize = Size.Empty
            };

            States.Add(root, state);

            root.SizeChanged += (_, _) => Apply(root);
            root.ParentChanged += (_, _) => Apply(root);
            root.HandleCreated += (_, _) => Apply(root);

            if (root is Form responsiveForm)
            {
                responsiveForm.Shown += (_, _) =>
                {
                    FitFormToScreen(responsiveForm, state.DesignSize);
                    Apply(root);
                };
            }

            Apply(root);
        }

        private static void CaptureSnapshots(Control parent, IDictionary<Control, ControlSnapshot> snapshots)
        {
            foreach (Control control in parent.Controls)
            {
                snapshots[control] = new ControlSnapshot
                {
                    Bounds = control.Bounds,
                    Margin = control.Margin,
                    Padding = control.Padding,
                    Font = control.Font,
                    AutoSize = control.AutoSize
                };

                CaptureSnapshots(control, snapshots);
            }
        }

        private static void NormalizeControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Dock == DockStyle.None)
                {
                    control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                }

                control.MinimumSize = Size.Empty;
                control.MaximumSize = Size.Empty;

                if (control is DataGridView grid)
                {
                    grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    grid.ScrollBars = ScrollBars.Both;
                    grid.AllowUserToResizeRows = false;
                    grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                }

                NormalizeControls(control);
            }
        }

        private static void Apply(Control root)
        {
            if (!States.TryGetValue(root, out var state))
            {
                return;
            }

            var currentSize = GetCurrentSize(root);
            if (currentSize.Width <= 0 || currentSize.Height <= 0 || currentSize == state.LastAppliedSize)
            {
                return;
            }

            state.LastAppliedSize = currentSize;

            var scaleX = ClampScale(currentSize.Width / (float)state.DesignSize.Width);
            var scaleY = ClampScale(currentSize.Height / (float)state.DesignSize.Height);
            var fontScale = ClampScale(Math.Min(scaleX, scaleY));

            root.SuspendLayout();
            try
            {
                ApplyRecursive(root, state.Snapshots, scaleX, scaleY, fontScale);
            }
            finally
            {
                root.ResumeLayout(true);
            }
        }

        private static void ApplyRecursive(
            Control parent,
            IReadOnlyDictionary<Control, ControlSnapshot> snapshots,
            float scaleX,
            float scaleY,
            float fontScale)
        {
            foreach (Control control in parent.Controls)
            {
                if (!snapshots.TryGetValue(control, out var snapshot))
                {
                    continue;
                }

                if (control.Dock == DockStyle.None)
                {
                    if (snapshot.AutoSize)
                    {
                        control.Location = new Point(
                            ScaleValue(snapshot.Bounds.X, scaleX),
                            ScaleValue(snapshot.Bounds.Y, scaleY));
                    }
                    else
                    {
                        control.Bounds = new Rectangle(
                            ScaleValue(snapshot.Bounds.X, scaleX),
                            ScaleValue(snapshot.Bounds.Y, scaleY),
                            Math.Max(1, ScaleValue(snapshot.Bounds.Width, scaleX)),
                            Math.Max(1, ScaleValue(snapshot.Bounds.Height, scaleY)));
                    }
                }

                control.Margin = ScalePadding(snapshot.Margin, scaleX, scaleY);
                control.Padding = ScalePadding(snapshot.Padding, scaleX, scaleY);
                control.Font = ScaleFont(snapshot.Font, fontScale);

                ApplyRecursive(control, snapshots, scaleX, scaleY, fontScale);
            }
        }

        private static void FitFormToScreen(Form form, Size designSize)
        {
            var workingArea = Screen.FromControl(form).WorkingArea;

            if (form.WindowState == FormWindowState.Maximized)
            {
                return;
            }

            var targetWidth = Math.Min(form.ClientSize.Width, Math.Max(workingArea.Width - 40, 320));
            var targetHeight = Math.Min(form.ClientSize.Height, Math.Max(workingArea.Height - 40, 320));

            if (designSize.Width > workingArea.Width - 40 || designSize.Height > workingArea.Height - 40)
            {
                targetWidth = Math.Max(320, workingArea.Width - 40);
                targetHeight = Math.Max(320, workingArea.Height - 40);
            }

            form.ClientSize = new Size(targetWidth, targetHeight);
        }

        private static Size GetDesignSize(Control root)
        {
            var size = root is Form ? root.ClientSize : root.Size;
            return new Size(Math.Max(size.Width, 1), Math.Max(size.Height, 1));
        }

        private static Size GetCurrentSize(Control root)
        {
            var size = root is Form ? root.ClientSize : root.Size;
            return new Size(Math.Max(size.Width, 0), Math.Max(size.Height, 0));
        }

        private static int ScaleValue(int value, float scale) =>
            (int)Math.Round(value * scale, MidpointRounding.AwayFromZero);

        private static Padding ScalePadding(Padding padding, float scaleX, float scaleY) =>
            new(
                ScaleValue(padding.Left, scaleX),
                ScaleValue(padding.Top, scaleY),
                ScaleValue(padding.Right, scaleX),
                ScaleValue(padding.Bottom, scaleY));

        private static Font ScaleFont(Font originalFont, float scale) =>
            new(
                originalFont.FontFamily,
                Math.Max(7F, originalFont.Size * scale),
                originalFont.Style,
                originalFont.Unit,
                originalFont.GdiCharSet,
                originalFont.GdiVerticalFont);

        private static float ClampScale(float value) => Math.Clamp(value, MinScale, MaxScale);
    }
}
