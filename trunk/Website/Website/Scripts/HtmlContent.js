$(document).ready(function () {
    $('.html-content-text').each(function () {
        var contentText = $(this);
        contentText.hide();
        contentText.ckeditor(
                function () {
                    $('.html-content-loader').hide();
                },
                {
                    height: (contentText.parent().height() - 137) + 'px',
                    resize_enabled: false
                });
    });
});
