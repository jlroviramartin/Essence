// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.ComponentModel;

namespace Essence.View.Views
{
    public interface IDialogView : IViewContainer
    {
        void Show();

        DialogResult ShowAsDialog();

        /// <summary>
        ///     Notifica que se esta cerrando el dialogo.
        /// </summary>
        event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        ///     Notifica que se ha cerrado el dialogo.
        /// </summary>
        event EventHandler Closed;
    }

    public enum DialogResult
    {
        Yes,
        No,
        Cancel
    }
}