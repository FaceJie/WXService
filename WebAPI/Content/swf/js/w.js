/*
* author: wx
* param: pick, 上传图片按钮的位置
* param: previewID, 图片容器
* param: server, 文件接收服务端。
* param: fileType, 指定允许选择的文件类型
* param: thumbnail, 预览图片的宽高
* param: fileCount 添加的文件数量
* param: fileSize 添加的文件总大小
* param: fileInformation 文件信息
* param: WCrate 创建Web Uploader实例的参数,   WCrate里面的属性请查看 webUpLoader 官网
*
* */
let Uploader = (function (window, $, undefined){
    function U(options) {
        this.options = options;
    }
    U.prototype.init = function (){
        // 配置
        let w = {

            // address:
            upLoaderID: '#uploader',                           // 上传图片功能容器
            previewID: '#filelist',                            // 图片容器
            statusBar: '#statusBar',                           // 状态栏，包括进度和控制按钮
            info: '#info',                                     // 文件总体选择信息。
            placeHolder: '#placeholder',
            progress: '#progress',

            // WCrate: webUpLoader 的配置暂时配置这些。如有需要请到官网查看 api
            swf: '../Uploader.swf',                             // swf文件路径
            server: './server/preview.php',                     // 服务地址
            pick: '#filePicker',                                // 初始化时候选择图片的地址
            thumbnail : [110, 110],                             // 缩略图大小
            resize: false,                                      // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
            accept: null,                                       // 设置允许上传的文件。指定接受哪些类型的文件
            dnd: '#placeholder',                                // 指定Drag And Drop拖拽的容器
            disableGlobalDnd: true,                             // 是否禁掉整个页面的拖拽功能
            paste: '#uploader',                                 // 指定监听paste事件的容器，此功能为通过粘贴来添加截屏的图片
            compress: false,                                    // 配置压缩的图片的选项。如果此选项为false, 则图片在上传前不进行压缩。
            chunked: false,
            chunkSize: 512 * 1024,
            auto: false,                                        // 设置为 true 后，不需要手动调用上传，有文件选择即开始上传。
            fileNumLimit: 6,                                    // 验证文件总数量, 超出则不允许加入队列
            fileSizeLimit: 100 * 1024 * 1024,                   // 100 M。验证文件总大小是否超出限制, 超出则不允许加入队列。
            fileSingleSizeLimit: 50 * 1024 * 1024,              // 50 M。验证单个文件大小是否超出限制, 超出则不允许加入队

            // info:
            fileCount: 0,
            fileSize: 0,
            percentages: {},                                    // 所有文件的进度信息，key为file id

            // btn:
            addButton:{
                id: '#picker',                             // 继续添加按钮
                label: '继续添加'
            },
            uploadBtn: '#uploadBtn'                             // 开始上传按钮
        };


        let isSupportBase64 = (function (){
                // 判断浏览器是否支持图片的base64
                let data = new Image();
                let support = true;
                data.onload = data.onerror = function() {
                    if( this.width != 1 || this.height != 1 ) {
                        support = false;
                    }
                };
                data.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
                return support;
            } )(),
            flashVersion = (function (){
                // 检测是否已经安装flash，检测flash的版本
                let version;
                try{
                    version = navigator.plugins[ 'Shockwave Flash' ];
                    version = version.description;
                }catch(ex){
                    try{
                        version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash')
                            .GetVariable('$version');
                    }catch(ex2){
                        version = '0.0';
                    }
                }
                version = version.match( /\d+/g );
                return parseFloat( version[ 0 ] + '.' + version[ 1 ], 10 );
            } )(),
            supportTransition = (function(){
                let s = document.createElement('p').style,
                    r = 'transition' in s ||
                        'WebkitTransition' in s ||
                        'MozTransition' in s ||
                        'msTransition' in s ||
                        'OTransition' in s;
                s = null;
                return r;
            })();


        // =============================================================================================================
        // 检测 flash
        if ( !WebUploader.Uploader.support('flash') && WebUploader.browser.ie ){
            // flash 安装了但是版本过低。
            if (flashVersion) {
                (function(container) {
                    window['expressinstallcallback'] = function( state ){
                        switch(state) {
                            case 'Download.Cancelled':
                                console.log('您取消了更新！');
                                break;
                            case 'Download.Failed':
                                console.log('安装失败');
                                break;
                            default:
                                console.log('安装已成功，请刷新！');
                                break;
                        }
                        delete window['expressinstallcallback'];
                    };
                    let swf = './expressInstall.swf';
                    // insert flash object
                    let html = '<object type="application/' +
                        'x-shockwave-flash" data="' +  swf + '" ';

                    if (WebUploader.browser.ie) {
                        html += 'classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" ';
                    }

                    html += 'width="100%" height="100%" style="outline:0">'  +
                        '<param name="movie" value="' + swf + '"/>' +
                        '<param name="wmode" value="transparent"/>' +
                        '<param name="allowscriptaccess" value="always"/>' +
                        '</object>';
                    container.html(html);
                })($wrap);
            } else {
                $wrap.html('<a href="http://www.adobe.com/go/getflashplayer" target="_blank" style="border:0;"><img alt="get flash player" src="http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg" /></a>');
            }
            return;
        } else if (!WebUploader.Uploader.support()) {
            console.log( 'Web Uploader 不支持您的浏览器！');
            return;
        }


        // =============================================================================================================
        $.extend(w, this.options);

        let uploader = WebUploader.create(w),
            $wrap = $(w.upLoaderID),
            $queue = $wrap.find(w.previewID),
            $statusBar = $wrap.find(w.statusBar),
            $info = $wrap.find(w.info),
            $progress = $wrap.find(w.progress),
            $upload = $wrap.find(w.uploadBtn),
            state = 'pedding',                                      // 可能有pedding, ready, uploading, confirm, done.
            $placeHolder = $wrap.find(w.placeHolder);
        $progress.hide();


        // =============================================================================================================
        // 拖拽时不接受 js, txt 文件。
        uploader.on('dndAccept', function (items){
            let denied = false,
                len = items.length,
                i = 0,
                // 修改js类型
                unAllowed = 'text/plain;application/javascript ';

            for ( ; i < len; i++ ) {
                // 如果在列表里面
                if (~unAllowed.indexOf(items[i].type)){
                    denied = true;
                    break;
                }
            }
            return !denied;
        });


        // =============================================================================================================
        // 添加“添加文件”的按钮，
        uploader.addButton({
            id: w.addButton.id,
            label: w.addButton.label,
        });


        // =============================================================================================================
        uploader.on('ready', function (){
            window.uploader = uploader;
        });


        // =============================================================================================================
        // 当有文件添加进来时执行，负责view的创建
        let addFile = function (file){
            let $li = $( '<li id="' + file.id + '">' +
                    '<p class="title">' + file.name + '</p>' +
                    '<p class="imgWrap"></p>'+
                    '<p class="progress"><span></span></p>' +
                    '</li>' ),

                $btns = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span>' +
                    '<span class="rotateRight">向右旋转</span>' +
                    '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
                $prgress = $li.find('p.progress span'),
                $wrap = $li.find( 'p.imgWrap' ),
                $info = $('<p class="error"></p>'),
                text = '',
                showError = function (code){
                    switch(code){
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;
                        case 'interrupt':
                            text = '上传暂停';
                            break;
                        default:
                            text = '上传失败，请重试';
                            break;
                    }
                    $info.text(text).appendTo($li);
                };

            if(file.getStatus() === 'invalid'){
                showError(file.statusText);
            }else{
                // @todo lazyload
                $wrap.text('预览中');
                uploader.makeThumb(file, function(error, src){
                    let img;
                    if(error){
                        $wrap.text('不能预览');
                        return;
                    }
                    if(isSupportBase64){
                        img = $('<img src="'+src+'">');
                        $wrap.empty().append( img );
                    }else{
                        $.ajax(w.server, {
                            method: 'POST',
                            data: src,
                            dataType:'json'
                        }).done(function(response){
                            if (response.result) {
                                img = $('<img src="'+response.result+'">');
                                $wrap.empty().append(img);
                            }else{
                                $wrap.text("预览出错");
                            }
                        });
                    }
                }, w.thumbnail[0], w.thumbnail[1]);

                w.percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function(cur, prev){
                if (prev === 'progress'){
                    $prgress.hide().width(0);
                }else if(prev === 'queued'){
                    $li.off('mouseenter mouseleave');
                    $btns.remove();
                }

                // 成功
                if (cur === 'error' || cur === 'invalid'){
                    showError(file.statusText);
                    w.percentages[file.id][1] = 1;
                }else if(cur === 'interrupt'){
                    showError( 'interrupt' );
                }else if( cur === 'queued'){
                    w.percentages[file.id][1] = 0;
                }else if(cur === 'progress'){
                    $info.remove();
                    $prgress.css('display', 'block');
                } else if(cur === 'complete'){
                    $li.append('<span class="success"></span>');
                }

                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });

            $li.on( 'mouseenter', function(){
                $btns.stop().animate({height: 30});
            });

            $li.on( 'mouseleave', function(){
                $btns.stop().animate({height: 0});
            });

            $btns.on( 'click', 'span', function(){
                let index = $(this).index(),
                    deg;
                switch (index){
                    case 0:
                        uploader.removeFile(file);
                        return;
                    case 1:
                        file.rotation += 90;
                        break;
                    case 2:
                        file.rotation -= 90;
                        break;
                }
                if (supportTransition){
                    deg = 'rotate(' + file.rotation + 'deg)';
                    $wrap.css({
                        '-webkit-transform': deg,
                        '-mos-transform': deg,
                        '-o-transform': deg,
                        'transform': deg
                    });
                }else{
                    $wrap.css( 'filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation='+ (~~((file.rotation/90)%4 + 4)%4) +')');
                }
            });
            $li.appendTo($queue);
        };


        // =============================================================================================================
        // 负责view的销毁
        let removeFile = function ( file ){
            let $li = $('#'+file.id);
            delete w.percentages[ file.id ];
            updateTotalProgress();
            $li.off().find('.file-panel').off().end().remove();
        };

        // =============================================================================================================
        let updateTotalProgress = function (){
            let loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(w.percentages, function(k, v){
                total += v[0];
                loaded += v[0] * v[1];
            } );

            percent = total ? loaded / total : 0;

            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        };

        // =============================================================================================================
        let updateStatus = function (){
            let text = '', stats;
            if (state === 'ready'){
                text = '选中 ' + w.fileCount + ' 张图片，共'+WebUploader.formatSize( w.fileSize ) + ' 。';
            }else if(state === 'confirm'){
                stats = uploader.getStats();
                if ( stats.uploadFailNum ){
                    text = '已成功上传' + stats.successNum+ '张照片，'+
                        stats.uploadFailNum + '张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片或<a class="ignore" href="#">忽略</a>'
                }
            }else{
                stats = uploader.getStats();
                text = '共 ' + w.fileCount + ' 张（' + WebUploader.formatSize( w.fileSize )  + '），已上传 ' + stats.successNum + ' 张。';
                if(stats.uploadFailNum){ text += '失败 ' + stats.uploadFailNum + ' 张'; }
            }
            $info.html(text);
        };

        // =============================================================================================================
        let setState = function (val){
            let file, stats;
            if (val === state){ return; }
            $upload.removeClass('state-' + state);
            $upload.addClass('state-' + val);
            state = val;

            switch (state){
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    $( '#filePicker2' ).removeClass('element-invisible');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'uploading':
                    $( '#filePicker2' ).addClass('element-invisible');
                    $progress.show();
                    $upload.text( '暂停上传' );
                    break;

                case 'paused':
                    $progress.show();
                    $upload.text( '继续上传' );
                    break;

                case 'confirm':
                    $progress.hide();
                    $( '#filePicker2' ).removeClass( 'element-invisible' );
                    $upload.text( '开始上传' );

                    stats = uploader.getStats();
                    if ( stats.successNum && !stats.uploadFailNum ) {
                        setState( 'finish' );
                        return;
                    }
                    break;

                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum ){
                        console.log('上传成功');
                    }else{
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }
            updateStatus();
        };

        // =============================================================================================================
        uploader.onUploadProgress = function( file, percentage ) {
            let $li = $('#'+file.id),
                $percent = $li.find('.progress span');

            $percent.css('width', percentage * 100 + '%');
            w.percentages[ file.id ][1] = percentage;
            updateTotalProgress();
        };

        // =============================================================================================================
        uploader.onFileQueued = function(file){
            w.fileCount++;
            w.fileSize += file.size;
            if (w.fileCount === 1){
                $placeHolder.addClass('element-invisible');
                $statusBar.show();
            }
            addFile( file );
            setState('ready');
            updateTotalProgress();
        };

        // =============================================================================================================
        uploader.onFileDequeued = function(file){
            w.fileCount--;
            w.fileSize -= file.size;
            if(!w.fileCount){
                setState('pedding');
            }
            removeFile(file);
            updateTotalProgress();

        };

        // =============================================================================================================
        uploader.on('all', function(type){
            let stats;
            switch(type){
                case 'uploadFinished':
                    setState('confirm');
                    break;
                case 'startUpload':
                    setState( 'uploading' );
                    break;
                case 'stopUpload':
                    setState('paused');
                    break;
            }
        });

        // =============================================================================================================
        uploader.onError = function(code) {
            console.log('error: ' + code);
        };

        // =============================================================================================================
        $upload.on('click', function(){
            if ($(this).hasClass('disabled')){ return false; }
            if (state === 'ready'){
                uploader.upload();
            }else if(state === 'paused'){
                uploader.upload();
            }else if(state === 'uploading'){
                uploader.stop();
            }
        });

        // =============================================================================================================
        $info.on( 'click', '.retry', function (){
            uploader.retry();
        } );

        // =============================================================================================================
        $info.on('click', '.ignore', function (){
            console.log('todo');
        } );

        // =============================================================================================================
        $upload.addClass('state-' + state);
        updateTotalProgress();

    };

    return U;
})(window, jQuery);
