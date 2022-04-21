// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var GravatarApiWrapper = (function (window, $) {
    return {
        InjectGravatarProfileData: function (profile) {
            console.log(profile)
            if (profile && profile.entry && profile.entry.length > 0) {
                var userProfile = profile.entry[0];
                var $nameContainer = $('.gravatar-name[data-gravatar-hash=' + userProfile.hash + ']');
                $nameContainer.text(userProfile.displayName)
                $nameContainer.show();
            }
        }
    }
})(window, jQuery, undefined)