using UnityEngine;

namespace UI
{
    public interface IDragbarPanel
    {
        GameObject GetObject();
        DragBarUIManager GetDragBar();
        void EnableDragBar(bool enable);
    }
}