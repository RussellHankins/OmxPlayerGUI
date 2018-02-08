using System;
using System.Collections.Generic;
/// <summary>
/// Resizes controls on a form or user control automatically when the form resizes.
/// </summary>
namespace OmxPlayerGui
{
	public class AspectRatio
    {
        /// <summary>
        /// Used to store information about the controls to resize.
        /// </summary>
        private struct ControlInfo
        {
            public double _DockLeft;
            public double _DockWidth;
            public double _DockTop;
            public double _DockHeight;
            public System.Windows.Forms.Control _Control;
            public double _OriginalLeft;
            public double _OriginalWidth;
            public double _OriginalTop;
            public double _OriginalHeight;
            public ControlInfo(double left, double width, double top, double height, System.Windows.Forms.Control control)
            {
                _DockLeft = left;
                _DockWidth = width;
                _DockTop = top;
                _DockHeight = height;
                _Control = control;
                _OriginalLeft = control.Left;
                _OriginalWidth = control.Width;
                _OriginalTop = control.Top;
                _OriginalHeight = control.Height;
            }
            public void Resize(double widthChange,double heightChange)
            {
                if (_DockLeft != 0)
                {
                    // DOCK LEFT.
                    _Control.Left = (int)(_OriginalLeft + widthChange * _DockLeft);
                }
                if (_DockWidth != 0)
                {
                    // DOCK RIGHT.
                    _Control.Width = (int)(_OriginalWidth + widthChange * _DockWidth);
                }
                if (_DockTop != 0)
                {
                    // DOCK TOP.
                    _Control.Top = (int)(_OriginalTop + heightChange * _DockTop);
                }
                if (_DockHeight != 0)
                {
                    // DOCK BOTTOM.
                    _Control.Height = (int)(_OriginalHeight + heightChange * _DockHeight);
                }
                return;
            }
        }

        private System.Windows.Forms.Form _ParentForm;
        private System.Windows.Forms.Control _ParentControl;
        private List<ControlInfo> _ControlInfo;
        private double _OriginalWidth;
        private double _OriginalHeight;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentForm">Form that has the controls to be resized</param>
        public AspectRatio(System.Windows.Forms.Form parentForm)
        {
            _ParentForm = parentForm;
            _ControlInfo = new List<ControlInfo>();
            _OriginalWidth = parentForm.Width;
            _OriginalHeight = parentForm.Height;
            parentForm.Resize += new System.EventHandler(this.ResizeEvent_Form);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentControl">Control that has the controls to be resized.</param>
        public AspectRatio(System.Windows.Forms.Control parentControl)
        {
            _ParentControl = parentControl;
            _ControlInfo = new List<ControlInfo>();
            _OriginalWidth = parentControl.Width;
            _OriginalHeight = parentControl.Height;
            parentControl.Resize += new System.EventHandler(this.ResizeEvent_Control);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlToAdd">Control to adjust when the form resizes</param>
        /// <param name="dockLeft">0 to leave Control.Left alone.1 - dock. 0.5 - center.</param>
        /// <param name="dockWidth">0 to leave Control.Width alone. 1 - dock. 0.5 - center</param>
        /// <param name="dockTop">0 to leave Control.Top alone. 1 - dock. 0.5 - center</param>
        /// <param name="dockHeight">0 to leave Control.Height alone. 1 - dock. 0.5 - center</param>
        public void AddControl(System.Windows.Forms.Control controlToAdd, double dockLeft, double dockWidth, double dockTop, double dockHeight)
        {
            // oControl IS THE CONTROL TO BE RESIZED WHEN THE FORM IS RESIZED.
            // bDockLeft: The Left property is changed.
            // bDockWidth: The Width property is changed.
            // bDockTop: The Top property is changed.
            // bDockHeight: The Height property is changed.
            if ((dockLeft==0) && (dockWidth==0) && (dockTop==0) && (dockHeight==0))
            {
                // DO NOTHING.
                return;
            }
            
            _ControlInfo.Add(new ControlInfo(dockLeft, dockWidth, dockTop, dockHeight, controlToAdd));
                    
            return;
        }
        /// <summary>
        /// This event is fired after the ResizeEvent_Form is finished resizing.
        /// </summary>
        public EventHandler AfterResize = null;
        private void ResizeEvent_Form(object sender, System.EventArgs e)
        {
            // CALLED FROM THE RESIZE EVENT OF A FORM.
            // THE CONTRUCTOR OF THIS CLASS AUTOMATICALLY
            // ADDS AN EVENT TO JUMP TO THIS FUNCTION FOR Form_Resize.
            ResizeControls(_ParentForm.Width - _OriginalWidth, _ParentForm.Height - _OriginalHeight,sender,e);
            return;
        }
        private void ResizeEvent_Control(object sender, System.EventArgs e)
        {
            // CALLED FROM THE RESIZE EVENT OF A CONTROL.
            // THE CONTRUCTOR OF THIS CLASS AUTOMATICALLY
            // ADDS AN EVENT TO JUMP TO THIS FUNCTION FOR Form_Resize.
            ResizeControls(_ParentControl.Width - _OriginalWidth, _ParentControl.Height - _OriginalHeight, sender, e);
            return;
        }
        private void ResizeControls(double widthChange, double heightChange, object sender, System.EventArgs e)
        {
            // Called from ResizeEvent_Control and ResizeEvent_Form.
            foreach (ControlInfo loop in _ControlInfo)
            {
                loop.Resize(widthChange, heightChange);
            }
            // Pass the resize event on.
            if (AfterResize != null)
            {
                AfterResize(sender, e);
            }
            return;
        }
	    }
}