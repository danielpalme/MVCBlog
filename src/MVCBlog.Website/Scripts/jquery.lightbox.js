/**
 * jQuery Lightbox
 * Version 0.5 - 11/29/2007
 * @author Warren Krewenki
 *
 * This package is distributed under the BSD license.
 * For full license information, see LICENSE.TXT
 *
 * Based on Lightbox 2 by Lokesh Dhakar (http://www.huddletogether.com/projects/lightbox2/)
 * Originally written to make use of the Prototype framework, and Script.acalo.us, now altered to use jQuery.
 *
 *
 **/

(function($){

    $.fn.lightbox = function(options){
        // build main options
        var opts = $.extend({}, $.fn.lightbox.defaults, options);
        
        return this.each(function(){
            $(this).click(function(){
                // initalize the lightbox
                initialize();
                start(this);
                return false;
            });
        });
        
        /**
         * initalize()
         *
         * @return void
         * @author Warren Krewenki
         */
         
        function initialize() {
            $('#overlay').remove();
            $('#lightbox').remove();
            opts.inprogress = false;
            
            // if jsonData, build the imageArray from data provided in JSON format
            if(opts.jsonData && opts.jsonData.length > 0) {
                var parser = opts.jsonDataParser ? opts.jsonDataParser : $.fn.lightbox.parseJsonData;                
                opts.imageArray = [];
                opts.imageArray = parser(opts.jsonData);
            }
            
            var outerImage = '<div id="outerImageContainer"><div id="imageContainer"><iframe id="lightboxIframe" /><img id="lightboxImage"><div id="hoverNav"><a href="javascript://" title="' + opts.strings.prevLinkTitle + '" id="prevLink"></a><a href="javascript://" id="nextLink" title="' + opts.strings.nextLinkTitle + '"></a></div><div id="loading"><a href="javascript://" id="loadingLink"><img src="'+opts.fileLoadingImage+'"></a></div></div></div>';
            var imageData = '<div id="imageDataContainer" class="clearfix"><div id="imageData"><div id="imageDetails"><span id="caption"></span><span id="numberDisplay"></span></div><div id="bottomNav">';

            if (opts.displayHelp)
                imageData += '<span id="helpDisplay">' + opts.strings.help + '</span>';

            imageData += '<a href="javascript://" id="bottomNavClose" title="' + opts.strings.closeTitle + '"><img src="'+opts.fileBottomNavCloseImage+'"></a></div></div></div>';

            var string;

            if (opts.navbarOnTop) {
              string = '<div id="overlay"></div><div id="lightbox">' + imageData + outerImage + '</div>';
              $("body").append(string);
              $("#imageDataContainer").addClass('ontop');
            } else {
              string = '<div id="overlay"></div><div id="lightbox">' + outerImage + imageData + '</div>';
              $("body").append(string);
            }

            $("#overlay").click(function(){ end(); }).hide();
            $("#lightbox").click(function(){ end();}).hide();
            $("#loadingLink").click(function(){ end(); return false;});
            $("#bottomNavClose").click(function(){ end(); return false; });
            $('#outerImageContainer').width(opts.widthCurrent).height(opts.heightCurrent);
            $('#imageDataContainer').width(opts.widthCurrent);
        
            if (!opts.imageClickClose) {
                $("#lightboxImage").click(function(){ return false; });
                $("#hoverNav").click(function(){ return false; });
            }
        };
        
        function getPageSize() {
            var jqueryPageSize = new Array($(document).width(),$(document).height(), $(window).width(), $(window).height());
            return jqueryPageSize;
        };
        
        function getPageScroll() {
            var xScroll, yScroll;

            if (self.pageYOffset) {
                yScroll = self.pageYOffset;
                xScroll = self.pageXOffset;
            } else if (document.documentElement && document.documentElement.scrollTop){  // Explorer 6 Strict
                yScroll = document.documentElement.scrollTop;
                xScroll = document.documentElement.scrollLeft;
            } else if (document.body) {// all other Explorers
                yScroll = document.body.scrollTop;
                xScroll = document.body.scrollLeft;
            }

            var arrayPageScroll = new Array(xScroll,yScroll);
            return arrayPageScroll;
        };
        
        function pause(ms) {
            var date = new Date();
            var curDate = null;
            do{curDate = new Date();}
            while(curDate - date < ms);
        };
        
        function start(imageLink) {
            $("select, embed, object").hide();
            var arrayPageSize = getPageSize();
            $("#overlay").hide().css({width: '100%', height: arrayPageSize[1]+'px', opacity : opts.overlayOpacity}).fadeIn();
            imageNum = 0;

            // if data is not provided by jsonData parameter
            if(!opts.jsonData) {
                opts.imageArray = [];
                // if image is NOT part of a set..
                if ((!imageLink.rel || (imageLink.rel == '')) && (!$(imageLink).data('rel') || $(imageLink).data('rel') == '')) {
                    // add single image to Lightbox.imageArray
                    opts.imageArray.push(new Array(imageLink.href, opts.displayTitle ? imageLink.title : ''));
                } else {
                // if image is part of a set..
                    $("a").each(function(){
                        if (this.href && ((this.rel && this.rel == imageLink.rel) || ($(imageLink).data('rel') && $(this).data('rel') == $(imageLink).data('rel')))) {
                            opts.imageArray.push(new Array(this.href, opts.displayTitle ? this.title : ''));
                        }
                    });
                }
            }
        
            if(opts.imageArray.length > 1) {
                for(i = 0; i < opts.imageArray.length; i++){
                    for(j = opts.imageArray.length-1; j>i; j--){
                        if(opts.imageArray[i][0] == opts.imageArray[j][0]){
                            opts.imageArray.splice(j,1);
                        }
                    }
                }
                while(opts.imageArray[imageNum][0] != imageLink.href) { imageNum++;}
            }

            // calculate top and left offset for the lightbox
            var arrayPageScroll = getPageScroll();
            var lightboxTop = arrayPageScroll[1] + (arrayPageSize[3] / 10);
            var lightboxLeft = arrayPageScroll[0];
            $('#lightbox').css({top: lightboxTop+'px', left: lightboxLeft+'px'}).show();


            if (!opts.slideNavBar)
                $('#imageData').hide();

            changeImage(imageNum);
        };
        
        function changeImage(imageNum) {
            if(opts.inprogress == false){
                opts.inprogress = true;
                opts.activeImage = imageNum;	// update global var

                // hide elements during transition
                $('#loading').show();
                $('#lightboxImage').hide();
                $('#hoverNav').hide();
                $('#prevLink').hide();
                $('#nextLink').hide();

                if (opts.slideNavBar) { // delay preloading image until navbar will slide up
                    // $('#imageDataContainer').slideUp(opts.navBarSlideSpeed, $.fn.doChangeImage);
                    $('#imageDataContainer').hide();
                    $('#imageData').hide();
                    doChangeImage();
                } else {
                    doChangeImage();
                }
            }
        };
        
        function doChangeImage() {

            imgPreloader = new Image();

            // once image is preloaded, resize image container
            imgPreloader.onload=function(){
                var newWidth = imgPreloader.width;
                var newHeight = imgPreloader.height;


                if (opts.fitToScreen) {
                    var arrayPageSize = getPageSize();
                    var ratio;
                    var initialPageWidth = arrayPageSize[2] - 2 * opts.borderSize;
                    var initialPageHeight = arrayPageSize[3] - 200;

                    var dI = initialPageWidth/initialPageHeight;
                    var dP = imgPreloader.width/imgPreloader.height;

                    if((imgPreloader.height > initialPageHeight) || (imgPreloader.width > initialPageWidth)){
                        if(dI > dP){
                            newWidth = parseInt((initialPageHeight/imgPreloader.height) * imgPreloader.width);
                            newHeight = initialPageHeight;
                        } else {
                            newHeight = parseInt((initialPageWidth/imgPreloader.width) * imgPreloader.height);
                            newWidth = initialPageWidth;
                        }
                    }
                }

                $('#lightboxImage').attr('src', opts.imageArray[opts.activeImage][0])
                                   .width(newWidth).height(newHeight);
                resizeImageContainer(newWidth, newHeight);
            };

            imgPreloader.src = opts.imageArray[opts.activeImage][0];
        };
        
        function end() {
            disableKeyboardNav();
            $('#lightbox').hide();
            $('#overlay').fadeOut();
            $('select, object, embed').show();
        };
        
        function preloadNeighborImages(){
            if(opts.loopImages && opts.imageArray.length > 1) {
                preloadNextImage = new Image();
                preloadNextImage.src = opts.imageArray[(opts.activeImage == (opts.imageArray.length - 1)) ? 0 : opts.activeImage + 1][0]
                
                preloadPrevImage = new Image();
                preloadPrevImage.src = opts.imageArray[(opts.activeImage == 0) ? (opts.imageArray.length - 1) : opts.activeImage - 1][0]
            } else {
                if((opts.imageArray.length - 1) > opts.activeImage){
                    preloadNextImage = new Image();
                    preloadNextImage.src = opts.imageArray[opts.activeImage + 1][0];
                }
                if(opts.activeImage > 0){
                    preloadPrevImage = new Image();
                    preloadPrevImage.src = opts.imageArray[opts.activeImage - 1][0];
                }
            }
        };
        
        function resizeImageContainer(imgWidth, imgHeight) {
            // get current width and height
            opts.widthCurrent = $("#outerImageContainer").outerWidth();
            opts.heightCurrent = $("#outerImageContainer").outerHeight();
            
            // get new width and height
            var widthNew = Math.max(350, imgWidth  + (opts.borderSize * 2));
            var heightNew = (imgHeight  + (opts.borderSize * 2));

            // scalars based on change from old to new
            opts.xScale = ( widthNew / opts.widthCurrent) * 100;
            opts.yScale = ( heightNew / opts.heightCurrent) * 100;

            // calculate size difference between new and old image, and resize if necessary
            wDiff = opts.widthCurrent - widthNew;
            hDiff = opts.heightCurrent - heightNew;

            $('#imageDataContainer').animate({width: widthNew},opts.resizeSpeed,'linear');
            $('#outerImageContainer').animate({width: widthNew},opts.resizeSpeed,'linear',function(){
                $('#outerImageContainer').animate({height: heightNew},opts.resizeSpeed,'linear',function(){
                    showImage();
                });
            });

            // if new and old image are same size and no scaling transition is necessary,
            // do a quick pause to prevent image flicker.
            if((hDiff == 0) && (wDiff == 0)){
                if (jQuery.browser.msie){ pause(250); } else { pause(100);}
            }

            $('#prevLink').height(imgHeight);
            $('#nextLink').height(imgHeight);
        };
        
        function showImage() {
            $('#loading').hide();
            $('#lightboxImage').fadeIn("fast");
            updateDetails();
            preloadNeighborImages();

            opts.inprogress = false;
        };
        
        function updateDetails() {

            $('#numberDisplay').html('');

            if(opts.imageArray[opts.activeImage][1]){
                $('#caption').html(opts.imageArray[opts.activeImage][1]).show();
            }

            // if image is part of set display 'Image x of x'
            if(opts.imageArray.length > 1){
                var nav_html;

                nav_html = opts.strings.image + (opts.activeImage + 1) + opts.strings.of + opts.imageArray.length;

                if (!opts.disableNavbarLinks) {
                    // display previous / next text links
                    if ((opts.activeImage) > 0 || opts.loopImages) {
                      nav_html = '<a title="' + opts.strings.prevLinkTitle + '" href="#" id="prevLinkText">' + opts.strings.prevLinkText + "</a>" + nav_html;
                    }

                    if (((opts.activeImage + 1) < opts.imageArray.length) || opts.loopImages) {
                      nav_html += '<a title="' + opts.strings.nextLinkTitle + '" href="#" id="nextLinkText">' + opts.strings.nextLinkText + "</a>";
                    }
                }

                $('#numberDisplay').html(nav_html).show();
            }

            if (opts.slideNavBar) {
                $("#imageData").slideDown(opts.navBarSlideSpeed);
            } else {
                $("#imageData").show();
            }

            var arrayPageSize = getPageSize();
            $('#overlay').height(arrayPageSize[1]);
            updateNav();
        };
        
        function updateNav() {
            if(opts.imageArray.length > 1){
                $('#hoverNav').show();
                
                // if loopImages is true, always show next and prev image buttons 
                if(opts.loopImages) {
                    $('#prevLink,#prevLinkText').show().click(function(){
                        changeImage((opts.activeImage == 0) ? (opts.imageArray.length - 1) : opts.activeImage - 1); return false;
                    });
                    
                    $('#nextLink,#nextLinkText').show().click(function(){
                        changeImage((opts.activeImage == (opts.imageArray.length - 1)) ? 0 : opts.activeImage + 1); return false;
                    });
                
                } else {
                    // if not first image in set, display prev image button
                    if(opts.activeImage != 0){
                        $('#prevLink,#prevLinkText').show().click(function(){
                            changeImage(opts.activeImage - 1); return false;
                        });
                    }

                    // if not last image in set, display next image button
                    if(opts.activeImage != (opts.imageArray.length - 1)){
                        $('#nextLink,#nextLinkText').show().click(function(){

                            changeImage(opts.activeImage +1); return false;
                        });
                    }
                }
                
                enableKeyboardNav();
            }
        };
        
        function keyboardAction(e) {
            var o = e.data.opts
            var keycode = e.keyCode;
            var escapeKey = 27;
            
            var key = String.fromCharCode(keycode).toLowerCase();
            
            if((key == 'x') || (key == 'o') || (key == 'c') || (keycode == escapeKey)){ // close lightbox
                end();
            } else if((key == 'p') || (keycode == 37)){ // display previous image
                if(o.loopImages) {
                    disableKeyboardNav();
                    changeImage((o.activeImage == 0) ? (o.imageArray.length - 1) : o.activeImage - 1);
                } 
                else if(o.activeImage != 0){
                    disableKeyboardNav();
                    changeImage(o.activeImage - 1);
                }
            } else if((key == 'n') || (keycode == 39)){ // display next image
                if (opts.loopImages) {
                    disableKeyboardNav();
                    changeImage((o.activeImage == (o.imageArray.length - 1)) ? 0 : o.activeImage + 1);
                }
                else if(o.activeImage != (o.imageArray.length - 1)){
                    disableKeyboardNav();
                    changeImage(o.activeImage + 1);
                }
            }
        };
        
        function enableKeyboardNav() {
            $(document).bind('keydown', {opts: opts}, keyboardAction);
        };

        function disableKeyboardNav() {
            $(document).unbind('keydown');
        };
        
    };
    
    $.fn.lightbox.parseJsonData = function(data) {
        var imageArray = [];
        
        $.each(data, function(){
            imageArray.push(new Array(this.url, this.title));
        });
        
        return imageArray;
    };

    $.fn.lightbox.defaults = {
        fileLoadingImage : 'gx/lightbox/loading.gif',
        fileBottomNavCloseImage: 'gx/lightbox/closelabel.gif',
        overlayOpacity : 0.8,
        borderSize : 10,
        imageArray : new Array,
        activeImage : null,
        inprogress : false,
        resizeSpeed : 350,
        widthCurrent: 250,
        heightCurrent: 250,
        xScale : 1,
        yScale : 1,
        displayTitle: true,
        navbarOnTop: false,
        slideNavBar: false, // slide nav bar up/down between image resizing transitions
        navBarSlideSpeed: 350,
        displayHelp: false,
        strings : {
            help: ' \u2190 / P - previous image\u00a0\u00a0\u00a0\u00a0\u2192 / N - next image\u00a0\u00a0\u00a0\u00a0ESC / X - close image gallery',
            prevLinkTitle: 'previous image',
            nextLinkTitle: 'next image',
            prevLinkText:  '&laquo; Previous',
            nextLinkText:  'Next &raquo;',
            closeTitle: 'close image gallery',
            image: 'Image ',
            of: ' of '
        },
        fitToScreen: false,		// resize images if they are bigger than window
        disableNavbarLinks: false,
        loopImages: false,
        imageClickClose: true,
        jsonData: null,
        jsonDataParser: null
    };
    
})(jQuery);