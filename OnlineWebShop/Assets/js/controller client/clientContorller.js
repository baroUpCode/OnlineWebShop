var product={
    init: function () { },
    //Bắt sự kiện khi được click vào và load thông tin ra , khi click vào Loại sp thì sẽ lấy được CatogoryID và show ra các sản phẩm có CatogoryID trả về
    registerEvent: function () {
        $('.item').off('click').on('click', function (e) {
            e.preventDefault();
        });
    }

}