var GravatarApiWrapper = (function (window) {
    return {
        InjectGravatarProfileData: function (profile) {
            if (profile && profile.entry && profile.entry.length > 0) {
                var userProfile = profile.entry[0];
                var nameContainers = document.querySelectorAll('.gravatar-name[data-gravatar-hash="' + userProfile.hash + '"]');
                for (const c of nameContainers) {
                    c.innerText = userProfile.displayName;
                    if (c.classList.contains("hide"))
                        c.classList.remove("hide")
                }
            }
        }
    }
})(window, undefined)