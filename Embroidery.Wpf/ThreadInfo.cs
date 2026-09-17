using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Embroidery.Wpf.models;

namespace Embroidery.Wpf.Controls;

public class ThreadInfo : FrameworkElement
{
    public PatternView? PatternView { get; set; }
    public event EventHandler<int?>? ThreadIndexChanged;
    private int? _threadIndexChanged;
    private Dictionary<byte, int> newDict;
    private Camera _camera = new Camera
    {
      CameraX = 0,
      CameraY = 0,
      Zoom = 10  
    };

    protected override void OnRender(DrawingContext dC)
    {
        base.OnRender(dC);
        
        if (PatternView is null)
        {
            return;
        }
        RenderOptions.SetBitmapScalingMode(
        this,
        BitmapScalingMode.NearestNeighbor);

         Rect rect = new Rect(
                25,
                0,
                25,
                25
            );
        System.Windows.Point point = new Point(
                65,
                0
            );
        
        newDict = PatternView.ThreadsQuantity.OrderBy(x => PatternView.Threads[x.Key].IdThread).ToDictionary(x => x.Key, x => x.Value);

        foreach (var thread in newDict)
        {
            if (_threadIndexChanged is null)
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
                if (_threadIndexChanged == thread.Key)
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
            rect.Y += 50;
            point.Y += 50;
        }
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

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        if (newDict is null)
        {
            return;
        }
        int? threadIndex = newDict.ElementAt((int)e.GetPosition(this).Y/50).Key;
        if (threadIndex == _threadIndexChanged)
        {
            threadIndex = null;
        }
        _threadIndexChanged = threadIndex;
        ThreadIndexChanged?.Invoke(this, _threadIndexChanged);
        InvalidateVisual();
    }
}