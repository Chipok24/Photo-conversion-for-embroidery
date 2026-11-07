using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Embroidery.Core.Models;
using Embroidery.Wpf.models;

namespace Embroidery.Wpf.Controls;

public class ThreadInfo : FrameworkElement
{
    public PatternView? PatternView { get; set; }
    public event EventHandler<int?>? ThreadIndexDisplay;
    public event EventHandler<sbyte?>? ThreadIndexChange;
    private int? _threadIndexDisplay;
    private Dictionary<byte, int> newDict;
    private sbyte? _threadIndexChange;
    private Pen _pen = new Pen(new SolidColorBrush(Colors.Black), 2);
    private SolidColorBrush _brush = new SolidColorBrush(Colors.White);
    private Camera _camera = new Camera
    {
      CameraX = 0,
      CameraY = 0,
      Zoom = 10  
    };

    protected override void OnRender(DrawingContext dC)
    {
        base.OnRender(dC);
        _brush.Freeze();
        _pen.Freeze();
        
        if (PatternView is null)
        {
            return;
        }
        RenderOptions.SetBitmapScalingMode(
        this,
        BitmapScalingMode.NearestNeighbor);

         Rect rect = new Rect(
                25,
                10,
                25,
                25
            );
        System.Windows.Point point = new Point(
                65,
                10
            );
        
        newDict = PatternView.ThreadsQuantity.OrderBy(x => PatternView.Threads[x.Key].IdThread).ToDictionary(x => x.Key, x => x.Value);

        dC.DrawRectangle(_brush, _pen, rect);
        FormattedText fT = new FormattedText("Удалить цвет", CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            14,
            Brushes.Black,
            1.0);

            dC.DrawText(fT, point);


        foreach (var thread in newDict)
        {
            rect.Y += 50;
            point.Y += 50;
            if (_threadIndexDisplay is null && _threadIndexChange is null)
            {
                if (_camera.Zoom < 20)
                {
                    dC.DrawRectangle(PatternView.Threads[thread.Key].Brush, null, rect);
                }
                else
                {
                    dC.DrawImage(PatternView.Threads[thread.Key].TestSymbol, rect);
                }
            }
            else
            {
                if (_threadIndexDisplay == thread.Key || _threadIndexChange == thread.Key)
                {
                    if (_camera.Zoom < 20)
                    {
                        dC.DrawRectangle(PatternView.Threads[thread.Key].Brush, null, rect);
                    }
                    else
                    {
                        dC.DrawImage(PatternView.Threads[thread.Key].TestSymbol, rect);
                    }
                }
                else
                {
                    if (_camera.Zoom < 20)
                    {
                        dC.DrawRectangle(PatternView.Threads[thread.Key].BrushTransparent, null, rect);
                    }
                    else
                    {
                        dC.DrawImage(PatternView.Threads[thread.Key].TestSymbolTransparent, rect);
                    }
                }
            }

            FormattedText formattedText = new FormattedText($"№{PatternView.Threads[thread.Key].IdThread} КОЛ: {thread.Value}", CultureInfo.InvariantCulture,
            FlowDirection.LeftToRight,
            new Typeface("Arial"),
            14,
            Brushes.Black,
            1.0);

            dC.DrawText(formattedText, point);
        }
    }

    public void ResetParameters()
    {
        _threadIndexChange = null;
        _threadIndexDisplay = null;
        InvalidateVisual();
    }

    public void SetZoom(int zoom)
    {
        _camera.Zoom += zoom;
        if (_camera.Zoom < 1)
        {
            _camera.Zoom = 1;
        }
        InvalidateVisual();
    }

    public void SetPatternVies(PatternView patternView)
    {
        PatternView = patternView;
        Height = PatternView.ThreadsQuantity.Count() * 50;
        InvalidateVisual();
    }

    public void SetInvalidateVisual()
    {
        InvalidateVisual();
    }

    public void RecalculateCountThread(Cell cell)
    {
        if (cell.ThreadIndex is byte threadIndex)
        {
            PatternView.ThreadsQuantity[threadIndex] += cell.IsCompleted ? -1 : 1;
        }
        InvalidateVisual();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        int positionElement = (int)e.GetPosition(this).Y/50 - 1;
        _threadIndexChange = null;
        if (newDict is null || positionElement == -1)
        {
            return;
        }
        int? threadIndex = newDict.ElementAt(positionElement).Key;
        if (threadIndex == _threadIndexDisplay)
        {
            threadIndex = null;
        }
        _threadIndexDisplay = threadIndex;
        ThreadIndexDisplay?.Invoke(this, _threadIndexDisplay);
        ThreadIndexChange?.Invoke(this, _threadIndexChange);
        InvalidateVisual();
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);
        int positionElement = (int)e.GetPosition(this).Y/50 - 1;
        if (newDict is null) 
        {
            return;
        }
        _threadIndexDisplay = null;

        sbyte? threadIndex;
        if (positionElement == -1)
        {
            threadIndex = -1;
        }
        else
        {
            threadIndex = (sbyte)newDict.ElementAt(positionElement).Key;
        }
        
        if (threadIndex == _threadIndexChange)
        {
            threadIndex = null;
        }
        _threadIndexChange = threadIndex;
        ThreadIndexChange?.Invoke(this, _threadIndexChange);
        ThreadIndexDisplay?.Invoke(this, _threadIndexDisplay);
        InvalidateVisual();
    }
}