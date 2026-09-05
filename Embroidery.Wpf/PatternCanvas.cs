
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
    private System.Windows.Point _lastMouse;
    private int _lastPositionCellX = -1;
    private int _lastPositionCellY = -1;
    private bool _initialCellState = false;

    protected override void OnRender(DrawingContext dC)
    {
        int renderObjects = 0;
        Stopwatch stopwatch = Stopwatch.StartNew();
        base.OnRender(dC);
        RenderOptions.SetBitmapScalingMode(
        this,
        BitmapScalingMode.NearestNeighbor);

        if (PatternView is null)
        {
            return;
        }

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

        double proc = _camera.Zoom * 0.20;
        double kletca = _camera.Zoom - proc * 2;

        Rect symbolRect = new Rect(
            0,
            0,
            kletca,
            kletca);

        

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

                if (_camera.Zoom < 20)
                {
                    if (PatternView.Cells[x, y].IsCompleted is true)
                    {
                        dC.DrawRectangle(PatternView.Threads[(int)threadId].BrushTransparent, null, rect);
                    }
                    else
                    {
                        dC.DrawRectangle(PatternView.Threads[(int)threadId].Brush, null, rect);
                    }

                    renderObjects++;
                }

                if (_camera.Zoom >= 20)
                {
                    //rect.X = rect.X;
                    //rect.Y = rect.Y;
                    if (PatternView.Cells[x, y].IsCompleted is true)
                    {
                        dC.DrawImage(PatternView.Threads[(int)threadId].TestSymbolTransparent, rect);
                    }
                    else
                    {
                        dC.DrawImage(PatternView.Threads[(int)threadId].TestSymbol, rect);
                    }
                    renderObjects++;
                }   


            }
        }

        //Debug.WriteLine($"Объектов отрисовано: {renderObjects}");
        stopwatch.Stop();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        
        int positionX = (int)Math.Floor((e.GetPosition(this).X + _camera.CameraX) / _camera.Zoom);
        int positionY = (int)Math.Floor((e.GetPosition(this).Y + _camera.CameraY) / _camera.Zoom);
        _lastPositionCellX = positionX;
        _lastPositionCellY = positionY;
        _initialCellState=PatternView.Cells[positionX, positionY].IsCompleted;
        PatternView.Cells[positionX, positionY].IsCompleted = !PatternView.Cells[positionX, positionY].IsCompleted;
        InvalidateVisual();
        base.OnMouseLeftButtonDown(e);
    }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Left)
        {
            _isDragging = true;
            _lastMouse = e.GetPosition(this);

            CaptureMouse();
        }
        

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        /* if (!_isDragging)
        {
            return;
        } */

        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            int positionX = (int)Math.Floor((e.GetPosition(this).X + _camera.CameraX) / _camera.Zoom);
            int positionY = (int)Math.Floor((e.GetPosition(this).Y + _camera.CameraY) / _camera.Zoom);

            if (positionX == _lastPositionCellX && positionY == _lastPositionCellY)
            {
                return;
            }
            _lastPositionCellX = positionX;
            _lastPositionCellY = positionY;
            Debug.WriteLine("WORK");
            if (positionX < 0 || positionY < 0 || PatternView.Cells.GetLength(0) <= positionX || PatternView.Cells.GetLength(1) <= positionY)
            {
                return;
            }


            if (!_initialCellState)
            {
                PatternView.Cells[positionX, positionY].IsCompleted = true;
                InvalidateVisual();
                return;
            }
                PatternView.Cells[positionX, positionY].IsCompleted = false;
                InvalidateVisual();
        }

        if (Mouse.MiddleButton == MouseButtonState.Pressed)
        {
            System.Windows.Point point = e.GetPosition(this);
            double dx = point.X - _lastMouse.X;
            double dy = point.Y - _lastMouse.Y;

            _camera.CameraX -= dx;
            _camera.CameraY -= dy;

            SetCamera(_camera);
            _lastMouse = point;
        }
        
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Left)
        {
            _lastPositionCellX = -1;
            _lastPositionCellY = -1;
            _isDragging = false;
            ReleaseMouseCapture();
        }

        base.OnMouseUp(e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        
        int zoom =  e.Delta/120;
        
        double patternX = (e.GetPosition(this).X + _camera.CameraX) / _camera.Zoom;
        double patternY = (e.GetPosition(this).Y + _camera.CameraY) / _camera.Zoom;

        _camera.Zoom += zoom;
        _camera.CameraX = patternX * _camera.Zoom - e.GetPosition(this).X;
        _camera.CameraY = patternY * _camera.Zoom - e.GetPosition(this).Y;
        
        
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