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
            $('#previous').attr('title', previousPost.html());
            $('#previous').fadeIn();
        }
        else {
            $('#previous').fadeOut();
        }
        nextPost = getNextPost();
        if (nextPost.length) {
            $('#next').attr('title', nextPost.html());
            $('#next').fadeIn();
        }
        else {
            $('#next').fadeOut();
        }
    }

    setPreviousAndNextPosts();

    $('#content').scroll(function () {
        setPreviousAndNextPosts();
    });

    $('#previous').click(function (e) {
        if (previousPost.length) {
            (function (scrollTo) {
                $('#content').animate({
                    scrollTop: $('#content').scrollTop() + scrollTo - 30
                }, Math.abs(scrollTo) / 2);
            })(previousPost.offset().top);
        }
        e.preventDefault();
    });

    $('#next').click(function (e) {
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
