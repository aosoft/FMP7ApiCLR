FMP7 SDK for .NET
=====
Copyright (C) 2010-2016 TAN-Y

FMP archive center http://archive.fmp.jp/  

[最新バイナリはこちら (Latest Release)](releases/latest) | [更新履歴](update.md)

## このライブラリについて

FMP7 SDK の仕様に基づいて、 C# で実装した .NET Framework 用の API セットです。  
.NET Framework から簡単に FMP7 の制御が行えます。
公開されている機能は全てアクセスできるようになっています。


## ターゲット

.NET Framework 2.0 ～ 4.0 に対応した開発環境で使用できます。ポインタでアクセスした方が効率がよいケースがあるので、ポインタが使用できる言語（C#など）を推奨します。

.NET 実装ですが、本 SDK は Windows 専用です。

1.2.5.0 より対象とする FMP7 のバージョンは 7.10a 以降になります。それ以前のバージョンでは使用できません。

1.2.8.0 より Unity に対応しています (後述) 。

## サンプルについて

簡易パラメータービューアーとプレイリスト再生を実装してみました。
あまり効率のよいやり方をしていませんので、使い方の参考程度にしてください。

## 通常の Windows アプリケーションでの利用

Binary 以下の dll を適当な位置 (アプリケーションプロジェクト下など) にコピーし、参照アセンブリに追加してください。

* .NET Framework 2.0 ～ 3.x →  Binary/CLR2/FMP7Api.CLR2.dll
* .NET Framework 4.x ～ →  Binary/CLR4/FMP7Api.CLR4.dll

ソースコードで組み込む場合は SDK の .csproj を参照してください。下記 "ソースコードについて" も参考にしてください。

## Unity での利用

本 SDK の全機能を Unity から使用することができます。  
動作確認は 5.4.3f1 で行っていますが、実装的にはかなり前のバージョンから利用できるはずです。

ビルド済 DLL をマネージドプラグインとして組み込むか、ソースコードを C# スクリプトとして利用してください。  

DLL を利用する場合、Project Settings - Player の "Api Compatibility Level" に応じて DLL を選択してください。

* .NET Framework 2.0 ～ 3.x  →  Binary/CLR2/FMP7Api.CLR2.dll
* .NET Framework 4.x ～ →  Binary/CLR4/FMP7Api.CLR4.dll

.NET 4 は 5.5 以降での対応となりますが、 2016/11 現在、テストビルドの Editor でのみ利用可能ですので通常は FMP7Api.CLR2.dll を利用してください。

ソースコードで組み込む場合、 Unity の C# コンパイルオプションで unsafe を有効にする必要があります。 DLL で組み込む場合は必須ではありません。


## ソースコードについて

ソースコードは完全に共有していますが、プロジェクトで切り分けています。

* FMP7Api.CLR2.dll (FMP7Api_CLR2.sln)  
  Visual Studio 2008 SP1 用プロジェクト
* FMP7Api.CLR4.dll (FMP7Api_CLR4.sln)  
  Visual Studio 2010 用プロジェクト

.NET Framework の署名がするように設定されていますので、そのままではビルドできません。  
署名はキーコンテナを利用していますが、キーコンテナの設定はプロジェクトのプロパティから設定できないので、直接 .csproj の XML を編集する必要があります。

キーコンテナ名は "TAN-Y" です。プロジェクトをそのままでビルドする場合は適当な署名をこのコンテナに登録してからビルドしてください。

詳細は .NET Framework に関する各種ドキュメントを参考にしてください。

## ライセンス

本ソフトウェアは zlib/libpng License で配布します。

クラスライブラリを利用したソフトウェアは上記ライセンスに従って自由に利用、配布をしてください。
サンプルプログラムについては参考程度に適当にご利用ください。


クラスライブラリは FMP7 SDK の各種ヘッダファイル、ソースコードを参考に実装しています。
FMP7 SDK の著作権は Guu 氏に所属します。


<pre>
Copyright (c) 2010-2016 TAN-Y
FMP7 SDK Copyright (c) 2010-2014 Guu

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not
claim that you wrote the original software. If you use this software
in a product, an acknowledgment in the product documentation would be
appreciated but is not required.

2. Altered source versions must be plainly marked as such, and must not be
misrepresented as being the original software.

3. This notice may not be removed or altered from any source
distribution.
</pre>
