using System;
using System.Windows.Forms;
using SharpDX.Windows;
using Linearstar.Windows.RawInput;

namespace Crucible;
using System.Diagnostics;
using System.Drawing;

static class Program
{
  /// <summary>
  /// Entry point of application.
  /// </summary>
  [STAThread]
  static void Main()
  {
    var form = new SD12Form();
    form.SetClientSize(1920, 1080);
    form.Show();
    try
    {
      var engine = new SD12Engine(form);
      engine.Run();
    }
    catch (Exception ex)
    {
      MessageBox.Show(ex.ToString(), "Fatal Error", MessageBoxButtons.OK);
    }
  }
}

public sealed class SD12Form : RenderForm
{
  // Hide useless properties.
  new bool IsFullscreen { get; }
  new bool AllowUserResizing { get; }

  Rectangle prevWinRect;
  Action<RawInputData> inputEvent;
  static Size minSize = new Size(400, 400);

  public SD12Form() : base()
  {
    Size = minSize;
    Text = "DefaultTitle";
    MinimumSize = minSize;
    SetWindowMode(false);
  }

  /// <summary>
  ///  ClientSize excludes border size. 
  /// </summary>
  public void SetClientSize(int width, int height) => ClientSize = new Size(width, height);

  public void SetWindowMode(bool isFullScreenForm, bool allowResizing = true)
  {
    var targetRect = new Rectangle();
    if (isFullScreenForm)
    {
      FormBorderStyle = FormBorderStyle.None;
      prevWinRect = new Rectangle(Location, ClientSize);
      targetRect = Screen.FromControl(this).Bounds;
    }
    else
    {
      base.MaximizeBox = allowResizing;
      FormBorderStyle = allowResizing ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
      targetRect = prevWinRect;
    }
    ClientSize = targetRect.Size;
    Location = targetRect.Location;
  }

  public void SetInputEvent(Action<RawInputData> act) => inputEvent = act;

  protected override void WndProc(ref Message m)
  {
    const int WM_INPUT = 0x00FF;
    const int WM_ENTERSIZEMOVE = 0x0231;
    const int WM_EXITSIZEMOVE = 0x0232;
    const int WM_MOVE = 0x0003;
    const int WM_MOVING = 0x0216;
    if (m.Msg == WM_INPUT)
    {
      var data = RawInputData.FromHandle(m.LParam);
      inputEvent?.Invoke(data);
    }
    else if (m.Msg == WM_MOVE || m.Msg == WM_MOVING)
    {
      // TODO: Update when windows is being resized and being moved.
    }
    base.WndProc(ref m);
  }
}