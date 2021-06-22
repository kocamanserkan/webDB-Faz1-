function alertMsg(msg, type) {

    if (type == 'yes') {
        $.toast({
            heading: 'İşlem Başarılı',
            text: msg,
            icon: 'success',
            loader: true,
            loaderBg: '#fff',
            showHideTransition: 'fade',
            hideAfter: 3000,
            allowToastClose: false,
            position: {
                right: 100,
                left: 0,
                top: 50
            },


        })
    } else {
        $.toast({
            heading: 'Hata',
            text: msg,
            icon: 'error',
            loader: true,
            loaderBg: '#fff',
            showHideTransition: 'plain',
            hideAfter: 3000,
            position: {
                right: 100,
                left: 0,
                top: 50
            }
        })
    }
}