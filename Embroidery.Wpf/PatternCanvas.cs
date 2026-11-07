
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Embroidery.Core.Models;
using Embroidery.Wpf.models;


namespace Embroidery.Wpf.Controls;
public class PatternCanvas : FrameworkElement
{
    private PatternView? PatternView { get; set; }
    private Camera _camera = new Camera
    {
        CameraX = 0,
        CameraY = 0,
        Zoom = 10
    };
    private System.Windows.Point _lastMouse;
    private int _lastPositionCellX = -1;
    private int _lastPositionCellY = -1;
    private bool _initialCellState = true;
    private int? _chosenThreadIndex;
    private sbyte? _threadIndexChange;
    public event EventHandler<Cell>? PatternViewChanged2;


    protected override void OnRender(DrawingContext dC)
    {
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

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                byte? threadId = PatternView.Cells[x, y].ThreadIndex;
                Cell cell = PatternView.Cells[x, y];
                if (threadId is null)
                {
                    continue;
                }

                rect.X = x * _camera.Zoom - _camera.CameraX;
                rect.Y = y * _camera.Zoom - _camera.CameraY;

                if (_camera.Zoom < 20)
                {
                    DrawingCell(dC, rect, cell, false, _chosenThreadIndex);
                }

                else
                {
                    DrawingCell(dC, rect, cell, true, _chosenThreadIndex);
                }   


            }
        }
        stopwatch.Stop();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        int positionX = (int)Math.Floor((e.GetPosition(this).X + _camera.CameraX) / _camera.Zoom);
        int positionY = (int)Math.Floor((e.GetPosition(this).Y + _camera.CameraY) / _camera.Zoom);
        _lastPositionCellX = positionX;
        _lastPositionCellY = positionY;

        if (_threadIndexChange is sbyte threadIndexChange)
        {
            ChangeColorCellOnMouseLeftButtonDown(positionX, positionY, threadIndexChange);
        }
        else
        {
            byte? threadindex = PatternView.Cells[positionX, positionY].ThreadIndex;
            if (threadindex is null)
            {
                return;
            }

            StainingCellOnMouseLeftButtonDown(positionX, positionY);
            PatternViewChanged2?.Invoke(this, PatternView.Cells[positionX, positionY]);
        }
        
        InvalidateVisual();
        
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Left)
        {
            _lastMouse = e.GetPosition(this);
            CaptureMouse();
        }

        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int positionX = (int)Math.Floor((e.GetPosition(this).X + _camera.CameraX) / _camera.Zoom);
        int positionY = (int)Math.Floor((e.GetPosition(this).Y + _camera.CameraY) / _camera.Zoom);
        if (positionX == _lastPositionCellX && positionY == _lastPositionCellY)
        {
            return;
        }
        if (positionX < 0 || positionY < 0 || PatternView.Cells.GetLength(0) <= positionX || PatternView.Cells.GetLength(1) <= positionY)
        {
            return;
        }
        if (Mouse.LeftButton == MouseButtonState.Pressed && _threadIndexChange is null)
        {
            if (PatternView.Cells[positionX, positionY].IsCompleted != _initialCellState)
            {
                byte? threadindex = PatternView.Cells[positionX, positionY].ThreadIndex;
                if (threadindex is null)
                {
                    return;
                }
            }
            
            if (_initialCellState == PatternView.Cells[positionX, positionY].IsCompleted)
            {
                return;
            }

            if (_initialCellState)
            {
                PatternView.Cells[positionX, positionY].IsCompleted = true;
            }
            else
            {
                PatternView.Cells[positionX, positionY].IsCompleted = false;
            }
            _lastPositionCellX = positionX;
            _lastPositionCellY = positionY;
            PatternViewChanged2?.Invoke(this, PatternView.Cells[positionX, positionY]);
            InvalidateVisual();
            
        }

        if (Mouse.LeftButton == MouseButtonState.Pressed && _threadIndexChange is sbyte threadIndexChange)
        {
            _lastPositionCellX = positionX;
            _lastPositionCellY = positionY;
            ChangeColorCellOnMouseLeftButtonDown(positionX, positionY, threadIndexChange);
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
            ReleaseMouseCapture();
        }

        base.OnMouseUp(e);
    }

    public void SetCamera(Camera camera)
    {
        _camera = camera;
        InvalidateVisual();
    }

    public void SetZoom(int zoom, System.Windows.Point point)
    {
        double patternX = (point.X + _camera.CameraX) / _camera.Zoom;
        double patternY = (point.Y + _camera.CameraY) / _camera.Zoom;

        _camera.Zoom += zoom;
        _camera.CameraX = patternX * _camera.Zoom - point.X;
        _camera.CameraY = patternY * _camera.Zoom - point.Y;
        
        
        if (_camera.Zoom < 1)
        {
            _camera.Zoom = 1;
        }
        SetCamera(_camera);
    }

    public void SetPatternVies(PatternView patternView)
    {
        PatternView = patternView;
        InvalidateVisual();
    }

    public void SetChosenThreadIndex(int? chosenThreadIndex)
    {
        _chosenThreadIndex = chosenThreadIndex;
        InvalidateVisual();
    }

    public void SetThreadIndexChange(sbyte? threadIndex)
    {
        _threadIndexChange = threadIndex;
        InvalidateVisual();
    }

    private void DrawingCell(DrawingContext dC, Rect rect, Cell cell, bool drawSymbol = false, int? chosenThreadIndex = null)
    {
        var thread = PatternView.Threads[(int)cell.ThreadIndex];

        if (drawSymbol)
        {
            if (cell.IsCompleted is true || (chosenThreadIndex is not null && thread != PatternView.Threads[chosenThreadIndex.Value]))
            {
                dC.DrawImage(thread.TestSymbolTransparent, rect);
            }
            else
            {
                dC.DrawImage(thread.TestSymbol, rect);
            }        
        }
        else
        {
            if (cell.IsCompleted is true || (chosenThreadIndex is not null && thread != PatternView.Threads[chosenThreadIndex.Value]))
            {
                dC.DrawRectangle(thread.BrushTransparent, null, rect);
            }
            else
            {
                dC.DrawRectangle(thread.Brush, null, rect);
            }
        }
    }
    
    public void ResetParameters()
    {
        _chosenThreadIndex = null;
        _threadIndexChange = null;
    }

    private void StainingCellOnMouseLeftButtonDown(int x, int y)
    {
        _initialCellState = !PatternView.Cells[x, y].IsCompleted;
        PatternView.Cells[x, y].IsCompleted = !PatternView.Cells[x, y].IsCompleted;
    }

    private void ChangeColorCellOnMouseLeftButtonDown(int x, int y, sbyte threadIndex)
    {
        if (!PatternView.Cells[x, y].IsCompleted)
        {
            PatternView.Cells[x, y].IsCompleted = true;
            PatternViewChanged2?.Invoke(this, PatternView.Cells[x, y]);
        }
        
        PatternView.Cells[x, y].IsCompleted = false;
        if (threadIndex == -1)
        {
            PatternView.Cells[x, y].ThreadIndex = null;
        }
        else
        {
            PatternView.Cells[x, y].ThreadIndex = (byte)threadIndex;
            PatternViewChanged2?.Invoke(this, PatternView.Cells[x, y]);
        }
    }
}