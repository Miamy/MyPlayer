# MyPlayer
Yes, it's one more music player for Android :)
Simple, lightweight and suitable for my purposes.

The player understands hierarcical folders structure `Artist` - `Album` - `Song` and builds playlist correspondingly. 
Music search starts from the single root folder e.g. `Music`.

## Features

- Works with local storage (like SD card) only!
- Supports MP3 and FLAC files (for a moment)
- Displays covers if present in album folder
- Lyrics search using [ChartLyrics Lyric API][chartlyrics]

## Third-party libraries used in

- [LibVCL][lib-vcl]
- [TagLib.Portable][taglib]
- [Newtonsoft.Json][new-json]



## License [![MIT license][license-img]][license-url]

>The [`MIT`][license-url] License (MIT)
>
>Copyright &copy; 2014-2021 Sebastian Hildebrandt, [+innovations](http://www.plus-innovations.com).
>
>Permission is hereby granted, free of charge, to any person obtaining a copy
>of this software and associated documentation files (the "Software"), to deal
>in the Software without restriction, including without limitation the rights
>to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
>copies of the Software, and to permit persons to whom the Software is
>furnished to do so, subject to the following conditions:
>
>The above copyright notice and this permission notice shall be included in
>all copies or substantial portions of the Software.
>
>THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
>IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
>FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
>AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
>LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
>OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
>THE SOFTWARE.
>
>Further details see [LICENSE](LICENSE) file.


[chartlyrics]: http://www.chartlyrics.com/api.aspx

[lib-vcl]: https://code.videolan.org/videolan/vlc-android
[taglib]: https://github.com/timheuer/taglib-sharp-portable
[new-json]: https://www.newtonsoft.com/json

[license-url]: https://github.com/sebhildebrandt/systeminformation/blob/master/LICENSE
[license-img]: https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square
