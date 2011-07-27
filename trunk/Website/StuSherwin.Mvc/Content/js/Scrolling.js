(function ($, document, window, undefined) {
    
    var Scrolling = {
        previousPost: null,
        nextPost: null,

        previousLink: $('#previous'),
        nextLink: $('#next'),

        htmlDecode: function (value) {
            return $('<div/>').html(value).text();
        },

        scrollTop: function () {
            var scrollTop = $('body,html,document').scrollTop();

            if(scrollTop == 0)
                scrollTop = $(window).scrollTop();

            return scrollTop;
        },

        setLink: function (postLink, post) {
            if (post.length) {
                postLink.attr('title', this.htmlDecode(('a', post).html()));
                postLink.fadeIn();
            }
            else {
                postLink.fadeOut();
            }
        },

        setPreviousPost: function () {
            var self = this;
            this.previousPost = $('#content h1')
                .filter(function () {
                    return $(this).offset().top < self.scrollTop() + 30;
                })
                .last();

            this.setLink(this.previousLink, this.previousPost);
        },

        setNextPost: function () {
            var self = this;
            this.nextPost = $('#content h1')
                .filter(function () {
                    return $(this).offset().top > self.scrollTop() + 31;
                })
                .first();

            this.setLink(this.nextLink, this.nextPost);
        },

        scrollToPost: function (post) {
            var self = this;
            if (post.length) {
                (function (scrollTo) {
                    $('body,html,document').animate({
                        scrollTop: scrollTo - 30
                    }, Math.abs(self.scrollTop() - scrollTo) / 2);
                })(post.offset().top);
            }
        },

        init: function () {
            var self = this;
            $(document).ready(function () {
                self.setPreviousPost();
                self.setNextPost();

                $(document).scroll(function () {
                    self.setPreviousPost();
                    self.setNextPost();
                });

                self.previousLink.click(function (e) {
                    self.scrollToPost(self.previousPost);
                    e.preventDefault();
                });

                self.nextLink.click(function (e) {
                    self.scrollToPost(self.nextPost);
                    e.preventDefault();
                });
            });            
        }
    };

    Scrolling.init();

})(jQuery, document, window);