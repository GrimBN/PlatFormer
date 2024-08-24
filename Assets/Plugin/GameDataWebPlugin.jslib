mergeInto(LibraryManager.library, {

  SaveToLocalStorage: function (str) {
    localStorage.setItem("data_PlatFormer", UTF8ToString(str));
  },

  GetFromLocalStorage: function () {

    var returnStr = localStorage.getItem("data_PlatFormer");
    if(!returnStr)
    {
        return null;
    }
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },
});