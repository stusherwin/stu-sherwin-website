Scrolling = function () {
    var previousPost;
    var nextPost;

    function getPreviousPost() {
        return $('#content h1')
                    .filter(function () {
                        return $(this).offset().top < 30;
                    })
                    .last();
    }

    function getNextPost() {
        return $('#content h1')
                    .filter(function () {
                        return $(this).offset().top > 31;
                    })
                    .first();
    }

    function setPreviousAndNextPosts() {
        previousPost = getPreviousPost();
        if (previousPost.length) {
            $('#previous-post').fadeIn();
        }
        else {
            $('#previous-post').fadeOut();
        }
        nextPost = getNextPost();
        if (nextPost.length) {
            $('#next-post').fadeIn();
        }
        else {
            $('#next-post').fadeOut();
        }
    }

    setPreviousAndNextPosts();

    $('#content').scroll(function () {
        setPreviousAndNextPosts();
    });

    $('#previous-post').click(function (e) {
        if (previousPost.length) {
            (function (scrollTo) {
                $('#content').animate({
                    scrollTop: $('#content').scrollTop() + scrollTo - 30
                }, Math.abs(scrollTo) / 2);
            })(previousPost.offset().top);
        }
        e.preventDefault();
    });

    $('#next-post').click(function (e) {
        if (nextPost.length) {
            (function (scrollTo) {
                $('#content').animate({
                    scrollTop: $('#content').scrollTop() + scrollTo - 30
                }, Math.abs(scrollTo) / 2);
            })(nextPost.offset().top);
        }
        e.preventDefault();
    });
};

$(document).ready(function () {
    Scrolling();
});
