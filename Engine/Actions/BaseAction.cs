using System;
using Engine.Models.Items;

namespace Engine.Actions
{
    public abstract class BaseAction
    {
        protected readonly Item _item;

        public event EventHandler<string> OnActionPerformed;

        protected BaseAction(Item item) => _item = item;

        protected void ReportResult(string result) => OnActionPerformed?.Invoke(this, result);
    }
}
