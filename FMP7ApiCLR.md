FMP7 SDK for .NET
=====
Copyright (C)2010-2012 TAN-Y

## このライブラリについて

FMP7 SDKの仕様に基づいて、C#で実装した.NET Framework用のAPIセットです。  
.NET Frameworkから簡単にFMP7の制御が行えます。
公開されている機能は全てアクセスできるようになっています。


## ターゲット

.NET Framework 2.0 ～ 4.0 に対応した開発環境で使用できます。
ポインタでアクセスした方が効率がよいケースがあるので、ポインタが使用できる
言語（C#など）を推奨します。

1.2.5.0 より対象とする FMP7 のバージョンは 7.10a 以降になります。それ以前のバージョンでは使用できません。


## バイナリ

二種類の .dll を用意しています。
使用される環境に合わせて選択してください。
機能的な差異はありません。コンパイルに使用した環境、.NET Framework のターゲット
バージョンが異なるだけです。

* FMP7Api.CLR2.dll  
  .NET Framework 2.0 以降用。
  Visual Studio では 2005 以降で使用できます。
  .NET 4 以降で開発される場合、特別な理由がなければ FMP7Api.CLR4.dll を使用してください。
* FMP7Api.CLR4.dll  
  .NET Framework 4.0 以降用。
  Visual Studio では 2010 以降で使用できます。

.exe ファイルはサンプルプログラムです。
クラスライブラリとして使用する場合には不要です。
.dll ファイルのみコピーして使用してください。

## サンプルについて

簡易パラメータービューアーとプレイリスト再生を実装してみました。
あまり効率のよいやり方をしていませんので、使い方の参考程度にしてください。


## ソースコードについて

ソースコードは完全に共有していますが、プロジェクトで切り分けています。

* FMP7Api.CLR2.dll (FMP7Api_CLR2.sln)  
  Visual Studio 2008 SP1 用プロジェクト
* FMP7Api.CLR4.dll (FMP7Api_CLR4.sln)  
  Visual Studio 2010 用プロジェクト

.NET Framework の署名がするように設定されていますので、そのままではビルドできません。  
署名はキーコンテナを利用していますが、キーコンテナの設定はプロジェクトのプロパティから設定できないので、直接 .csproj の XML を編集する必要があります。  
詳細は .NET Framework に関する各種ドキュメントを参考にしてください。

## ライセンス

本ソフトウェアは zlib/libpng License で配布します。

クラスライブラリを利用したソフトウェアは上記ライセンスに従って自由に利用、配布をしてください。
サンプルプログラムについては参考程度に適当にご利用ください。


クラスライブラリは FMP7 SDK の各種ヘッダファイル、ソースコードを参考に実装しています。
FMP7 SDKの著作権は Guu 氏に所属します。


<pre>
Copyright (c) 2010-2012 TAN-Y
FMP7 SDK Copyright (c) 2010-2012 Guu

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

## 履歴

### Ver.1.2.6.0 2012/3/17
* exFMP7 専用 API に対応。
* exPMD 専用 API に対応。
* FMP4Control.GetFilePath でテキストデータがない時は null を返すようにした。

### Ver.1.2.5.0 2012/1/25
* FMP7 SDK 7.10a 対応。ワークの構造なども追従。
* FMP7 からのブロードキャストメッセージに対応。
* exFMP4 専用 API に対応。
* FMP7 のバージョン確認処理が不十分だったので修正。

### Ver.1.2.4.0
* FMP7 SDK 7.10a 対応の非公開テスト版

### Ver.1.1.3.0 2011/9/22
* FMP7 SDK 7.06c 対応。
* FMPConrrol.GetTextData でテキストデータがない場合は null を返すようにした。

### Ver.1.0.2.0
* FMPPartWork の State が ushort 型になっていたので FMPPartStat 型に修正。

### Ver.1.0.1.0
* 初版。
