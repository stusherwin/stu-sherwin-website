(function ($, document, window, undefined) {
    var ScrollLink = function (link, currentPost, pageScrollTop) {
        var link = link;
        var currentPost = currentPost;
        var pageScrollTop = pageScrollTop;

        var htmlDecode = function (value) {
            return $('<div/>').html(value).text();
        };

        var showLink = function () {
            if (currentPost.length) {
                link.attr('title', htmlDecode(('a', currentPost).html()));
                link.fadeIn();
            }
            else {
                link.fadeOut();
            }
        };

        var scroll = function () {
            if (currentPost.length) {
                (function (scrollTo) {
                    $('body,html,document').animate({
                        scrollTop: scrollTo - 30
                    }, Math.abs(pageScrollTop() - scrollTo) / 2);
                })(currentPost.offset().top);
            }
        };

        this.setCurrentPost = function (post) {
            currentPost = post;
            showLink();
        };

        this.init = function () {
            link.click(function (e) {
                scroll();
                e.preventDefault();
            });
        }
    };

    var Scrolling = function () {
        var currentScrollPosition = function () {
            var currentScrollPosition = $('body,html,document').scrollTop();

            if (currentScrollPosition == 0)
                currentScrollPosition = $(window).scrollTop();

            return currentScrollPosition;
        };

        var previousPostLink = new ScrollLink($('#previous'), $(null), currentScrollPosition);
        var nextPostLink = new ScrollLink($('#next'), $(null), currentScrollPosition);

        var setPreviousPost = function () {
            var previousPost = $('#content h1')
                .filter(function () {
                    var postScrollPosition = $(this).offset().top
                    return postScrollPosition < currentScrollPosition() + 30;
                })
                .last();

            previousPostLink.setCurrentPost(previousPost);
        };

        var setNextPost = function () {
            var nextPost = $('#content h1')
                .filter(function () {
                    var postScrollPosition = $(this).offset().top
                    return postScrollPosition > currentScrollPosition() + 31;
                })
                .first();

            nextPostLink.setCurrentPost(nextPost);
        };

        this.init = function () {
            previousPostLink.init();
            nextPostLink.init();

            $(document).scroll(function () {
                setPreviousPost();
                setNextPost();
            });

            setPreviousPost();
            setNextPost();
        };
    };

    var s = new Scrolling();

    $(document).ready(function () {
        s.init();
    });

})(jQuery, document, window);