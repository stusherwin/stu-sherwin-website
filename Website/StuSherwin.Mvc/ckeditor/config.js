/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.stylesSet =
    [
        { name: 'Heading 2', element: 'h2' },
        { name: 'Heading 3', element: 'h3' },
        { name: 'Heading 4', element: 'h4' },
        { name: 'Code', element: 'pre' },
    ];

    config.toolbar = 'Limited';

    config.toolbar_Limited =
    [
	    ['Source', '-', 'Maximize', 'ShowBlocks'],
        ['Cut', 'Copy', 'Paste', 'PasteText', '-', 'Undo', 'Redo'],
        ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'],
        '/',
	    ['Styles', 'Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript'],
	    ['NumberedList', 'BulletedList', '-', 'Blockquote', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
	    ['Link', 'Unlink', 'Anchor'],
   	    ['RemoveFormat']
    ];


};