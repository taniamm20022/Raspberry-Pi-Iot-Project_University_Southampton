$(document).ready(
    function () {

        $("p.topp button.btn.btn-primary.bFirstPage, p.bottom button.btn.btn-primary.bFirstPage").click(function () {
            $("#CurrentPage").val(1);
            $("#fSearch").submit();
        });

        $(".paginate_button.pagenumber").click(function () {
            var link = $(this).find("a");
            var pagenumber = $(link).text();

            $("#CurrentPage").val(pagenumber);
            $("#fSearch").trigger('submit');
        });

        $("#bPrevPage").click(function (evt) {
            $("#CurrentPage").val(parseInt($("#CurrentPage").val()) - 1);
            $("#fSearch").submit();
        });

        $("#bNextPage").click(function () {
            $("#CurrentPage").val(parseInt($("#CurrentPage").val()) + 1);
            $("#fSearch").submit();
        });


        $("#bLastPage").click(function () {
            $("#CurrentPage").val(parseInt($("#TotalPages").val()));
            $("#fSearch").submit();
        });
        $("#bFirstPage").click(function () {
            $("#CurrentPage").val(1);
            $("#fSearch").submit();
        });
        
    });