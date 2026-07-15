
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Embroidery.Core.Models;
using Embroidery.Wpf.models;


namespace Embroidery.Wpf.Controls;
public class PatternCanvas : FrameworkElement
{
    public PatternView? PatternView { get; set; }
    private Camera _camera = new Camera
    {
        CameraX = 0,
        CameraY = 0,
        Zoom = 10
    };
    private bool _isDragging;
    private Point _lastMouse;

    protected override void OnRender(DrawingContext dC)
    {
        Debug.WriteLine("Render");
        base.OnRender(dC);

        if (PatternView is null)
        {
            return;
        }

        //Pen pen = new Pen(Brushes.Black, 1);

        Rect rect = new Rect(
                    _camera.CameraX,
                    _camera.CameraY,
                    _camera.Zoom,
                    _camera.Zoom
                );

        int startX = (int)Math.Floor(_camera.CameraX/_camera.Zoom);
        int endX = (int)Math.Ceiling((_camera.CameraX+ActualWidth)/_camera.Zoom);

        int startY = (int)Math.Floor(_camera.CameraY/_camera.Zoom);
        int endY = (int)Math.Ceiling((_camera.CameraY+ActualHeight)/_camera.Zoom);

        if (endX > PatternView.Cells.GetLength(0))
        {
            endX = PatternView.Cells.GetLength(0);
        }

        if (endY > PatternView.Cells.GetLength(1))
        {
            endY = PatternView.Cells.GetLength(1);
        }

        startX = Math.Max(0, startX);
        startY = Math.Max(0, startY);

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                byte? threadId = PatternView.Cells[x, y].ThreadIndex;
                if (threadId is null)
                {
                    continue;
                }

                rect.X = x * _camera.Zoom - _camera.CameraX;
                rect.Y = y * _camera.Zoom - _camera.CameraY;

                dC.DrawRectangle(PatternView.Threads[(int)threadId].Brush, null, rect);
            }
        }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle)
        {
            _isDragging = true;
            _lastMouse = e.GetPosition(this);

            CaptureMouse();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        Point point = e.GetPosition(this);
        double dx = point.X - _lastMouse.X;
        double dy = point.Y - _lastMouse.Y;

        _camera.CameraX -= dx;
        _camera.CameraY -= dy;

        SetCamera(_camera);
        _lastMouse = point;
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle)
        {
            _isDragging = false;
            ReleaseMouseCapture();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        
        int zoom =  e.Delta/120;
        _camera.Zoom += zoom;
        if (_camera.Zoom < 1)
        {
            _camera.Zoom = 1;
        }
        SetCamera(_camera);
        base.OnMouseWheel(e);
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
        InvalidateVisual();
    }

    public void SetPatternVies(PatternView patternView)
    {
        PatternView = patternView;
        InvalidateVisual();
    }
}