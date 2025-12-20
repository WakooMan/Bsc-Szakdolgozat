using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.GameEngine
{
    public interface IInputManager
    {
        void KeyPressed(long keyCode);
        void KeyUp(long keyCode);
        void KeyDown(long keyCode);
        void MouseUp(MouseButton mouseButton, int x, int y);
        void MouseDown(MouseButton mouseButton, int x, int y);
        void MouseMove(int oldX, int oldY, int newX, int newY);

        void SubscribeKeyEvent(KeyEvent keyEvent, long keyCode, Action action);
        void SubscribeMouseEvent(MouseEvent mouseEvent, MouseButton mouseButton, Action<InputEventArgs> action);
        void UnsubscribeMouseEvent(MouseEvent mouseEvent, MouseButton mouseButton, Action<InputEventArgs> action);
    }
}
