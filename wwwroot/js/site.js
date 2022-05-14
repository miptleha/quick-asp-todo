$('#add-new').click(function () {
    var $div = $('#card-items').children().eq(0).clone();

    var i = $('#card-items').children().length;

    $div.find("input[name^='text-']").attr('name', 'text-' + i).val('');
    var $cb = $div.find("input[name^='done-']").attr('name', 'done-' + i).attr('id', 'check-' + i);
    if ($cb.length > 0)
        $cb[0].checked = false;
    $div.find("label[for^='check-']").attr('for', 'check-' + i);


    $('#card-items').append($div);
    return false;
});

$('#sel-category').change(function () {
    var cat = $(this).find('option:selected').text();
    setCategory(cat);
});

function setCategory(cat) {
    if (cat.length > 0) {
        var url = window.location.href;
        var index = url.indexOf("?category=");
        if (index >= 0) {
            url = url.substring(0, index);
        }
        if (cat != 'none') {
            url = url + "?category=" + cat;
        }
        window.location.href = url;
    }
}

$('body').on('click', '.delete-btn', function () {
    var $p = $(this).parent().parent();
    if ($p.parent().children().length > 1) 
        $p.remove();
    return false;
});

$('body').on('click', '.saved-checkbox', function () {
    var id = $(this).attr('id');
    var checked = $(this)[0].checked;
    var url = $('#url-link').attr('href');
    var ids = id.split('-');
    var t = $("input[name='__RequestVerificationToken']").val();
    if (ids.length > 2) {

        $.ajax({
            url: url + (url.indexOf("?") == -1 ? "?" : "&") + "handler=Check",
            type: 'Post',
            headers:
            {
                "RequestVerificationToken": t
            },
            data: {
                id1: ids[1],
                id2: ids[2],
                check: checked
            }
        });
    }
});

$('.mainNav > li').click(function (e) {
    $('.mainNav > li').removeClass('active');

    $(this).addClass('active');
    var cat = $(this).text();
    setCategory(cat);
});