var TodoDetailsPage = (function (window, $) {

    // #region Private Variables
    var _baseConfig = {
        endpointUrls: {
            createApi: '/TodoItem/ApiCreate',
            updateRankApi: '/TodoItem/ApiUpdateRank',
            reorderApi: '/TodoItem/ApiReorder'
        },
        resources: {
            genericErrorMessage: 'Unexpected error has occurred, please try again later.'
        },
        ids: {
            spinner: 'spinner',
            addNewItem: 'add-new-item',
            detailsContainer: 'details-container',
            hideDoneCheckbox: 'hide-done-items',
            linkOrderBy: 'link-order-by',
            form: {
                id: 'create-form',
                title: 'Title',
                importance: 'Importance',
                responsible: 'Responsible',
                rank: 'Rank',
                todoListId: 'TodoListId',
                todoItemId: 'TodoItemId'
            }
        },
        classes: {
            active: 'active',
            changeRank: 'change-rank',
            todoItem: 'todo-item',
            hide: 'hide',
            formGroup: 'form-group'
        }
    },
        _settings,
        _orderByRank = false,
        _todoListId = 0,
        _isSendingRequest = false,
        _editedItemId = null
    // #endregion

    // #region Private Methods

    function ToggleSpinner() {
        $('#' + _settings.ids.spinner).toggleClass(_settings.classes.active);
    }

    function ClearValidation($formElement) {
        var validator = $formElement.validate();
        $('[name]', $formElement.get(0)).each(function () {
            validator.successList.push(this);
            validator.showErrors();
        });
        validator.resetForm();
        validator.reset();
    }

    function ClearForm() {
        $('#' + _settings.ids.form.title).val('');
        $('#' + _settings.ids.form.importance + ' option:first').prop('selected', true);
        $('#' + _settings.ids.form.responsible + ' option:first').prop('selected', true);
        $('#' + _settings.ids.form.rank).val(0);
        var $form = $('#' + _settings.ids.form.id);
        ClearValidation($form);
        $form.find('.' + _settings.classes.formGroup).removeClass(_settings.classes.hide);
    }

    function PrepareFormForRankEdit(rank) {
        $('#' + _settings.ids.form.title).closest('.' + _settings.classes.formGroup).addClass(_settings.classes.hide);
        $('#' + _settings.ids.form.importance).closest('.' + _settings.classes.formGroup).addClass(_settings.classes.hide);
        $('#' + _settings.ids.form.responsible).closest('.' + _settings.classes.formGroup).addClass(_settings.classes.hide);
        $('#' + _settings.ids.form.rank).val(rank);
    }

    function GetFormDataObject() {
        var result = {};
        result[_settings.ids.form.title] = $('#' + _settings.ids.form.title).val();
        result[_settings.ids.form.importance] = $('#' + _settings.ids.form.importance).val();
        result[_settings.ids.form.responsible] = $('#' + _settings.ids.form.responsible).val();
        result[_settings.ids.form.rank] = $('#' + _settings.ids.form.rank).val();
        result[_settings.ids.form.todoListId] = _todoListId;
        if (_editedItemId != null)
            result[_settings.ids.form.todoItemId] = _editedItemId;
        return result;
    }

    function ChangeDetailsContent(newContent) {
        $('#' + _settings.ids.detailsContainer).html(newContent);
    }

    function AjaxCall(method, url, data, successFunction) {
        if (_isSendingRequest)
            return;

        $.ajax({
            type: method,
            url: url,
            data: data,
            beforeSend: function () {
                ToggleSpinner()
                _isSendingRequest = true;
            }
        }).done(function (response) {
            if (typeof successFunction === 'function')
                successFunction(response, data)
        }).fail(function () {
            alert(_settings.resources.genericErrorMessage)
        }).always(function () {
            ToggleSpinner();
            _isSendingRequest = false;
        });
    }

    // #endregion

    // #region Event Handlers

    function AddNewItem_Click() {
        ClearForm();

        $.validator.unobtrusive.parseElement($('#' + _settings.ids.form.rank).get(0));

        _editedItemId = null;

        $.fancybox.open({
            src: '#' + _settings.ids.form.id,
            width: 640,
            height: 480
        })
    }

    function ChangeRank_Click(e) {
        var $container = $(e.target).closest('.' + _settings.classes.todoItem);

        var rank = $container.attr('data-todo-rank');
        _editedItemId = $container.attr('data-todo-id');

        ClearForm();

        $.validator.unobtrusive.parseElement($('#' + _settings.ids.form.rank).get(0));

        PrepareFormForRankEdit(rank);

        $.fancybox.open({
            src: '#' + _settings.ids.form.id,
            width: 640,
            height: 240
        })
    }

    function FormSubmit_Click(e) {
        e.preventDefault();
        var $form = $(e.target).closest('form');
        if ($form.valid()) {
            var data = { fields: GetFormDataObject(), orderByRank: _orderByRank, hideDoneItems: $('#' + _settings.ids.hideDoneCheckbox).is(":checked") };

            var endpoint = (_editedItemId != null) ? _settings.endpointUrls.updateRankApi : _settings.endpointUrls.createApi;

            AjaxCall("POST", endpoint, data, ToDoItem_CreatedOrUpdated)
        }
    }

    function LinkOrderBy_Click(e) {
        var orderByRank = $('#' + _settings.ids.linkOrderBy).attr('data-todo-orderbyrank');
        var data = { todoListId: _todoListId, orderByRank: orderByRank, hideDoneItems: $('#' + _settings.ids.hideDoneCheckbox).is(":checked") };
        AjaxCall("GET", _settings.endpointUrls.reorderApi, data, ToDoItems_Reordered)
    }

    function ToDoItem_CreatedOrUpdated(response) {
        ChangeDetailsContent(response);
        $.fancybox.close();
    }

    function ToDoItems_Reordered(response) {
        ChangeDetailsContent(response);
        _orderByRank = !_orderByRank;
    }

    function InitEventHandlers() {
        $('body').on('click', '#' + _settings.ids.addNewItem, AddNewItem_Click);
        $('body').on('click', '.' + _settings.classes.changeRank, ChangeRank_Click);
        $('body').on('click', '#' + _settings.ids.linkOrderBy, LinkOrderBy_Click);
        $('#' + _settings.ids.form.id + ' button[type="submit"]').on('click', FormSubmit_Click);
    }

    // #endregion

    return {
        Init: function (settings, orderByRank, todoListId) {
            _settings = $.extend(true, {}, _baseConfig, settings);
            _orderByRank = orderByRank;
            _todoListId = todoListId;
            InitEventHandlers();
        }
    }
})(window, jQuery, undefined);