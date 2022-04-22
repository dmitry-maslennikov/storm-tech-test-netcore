// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var TodoDetailsPage = (function (window, $) {

    // #region Private Variables
    var _baseConfig = {
        endpointUrls: {
            createApi: '/TodoItem/CreateApi'
        },
        resources: {
            genericErrorMessage: 'Unexpected error has occurred, please try again later.'
        },
        ids: {
            spinner: 'spinner',
            addNewItem: 'add-new-item',
            detailsContainer: 'details-container',
            hideDoneCheckbox: 'hide-done-items',
            form: {
                id: 'create-form',
                title: 'Title',
                importance: 'Importance',
                responsible: 'Responsible',
                rank: 'Rank',
                todoListId: 'TodoListId'
            }
        },
        classes: {
            active: 'active'
        }
    },
        _settings,
        _orderByRank = false,
        _todoListId = 0,
        _isSendingRequest = false;
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
    }

    function GetFormDataObject() {
        var result = {};
        result[_settings.ids.form.title] = $('#' + _settings.ids.form.title).val();
        result[_settings.ids.form.importance] = $('#' + _settings.ids.form.importance).val();
        result[_settings.ids.form.responsible] = $('#' + _settings.ids.form.responsible).val();
        result[_settings.ids.form.rank] = $('#' + _settings.ids.form.rank).val();
        result[_settings.ids.form.todoListId] = _todoListId;
        return result;
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

        $.fancybox.open({
            src: '#' + _settings.ids.form.id,
            width: 640,
            height: 480
        })
    }

    function ToDoItem_Created(response, data) {
        $('#' + _settings.ids.detailsContainer).html(response);
        $.fancybox.close();
    }

    function CreateForm_Submit(e) {
        e.preventDefault();
        var $form = $(e.target).closest('form');
        if ($form.valid()) {
            var data = { fields: GetFormDataObject(), orderByRank: _orderByRank, hideDoneItems: $('#' + _settings.ids.hideDoneCheckbox).is(":checked") };
            AjaxCall("POST", _settings.endpointUrls.createApi, data, ToDoItem_Created)
        }
    }

    function InitEventHandlers() {
        $('body').on('click', '#' + _settings.ids.addNewItem, AddNewItem_Click);
        $('#' + _settings.ids.form.id + ' button[type="submit"]').on('click', CreateForm_Submit);
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