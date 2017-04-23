## FMP7 SDK for .NET 更新履歴

[README](README.md)

### Ver.1.2.9.0 2017/4/23
* FMPWork.Open でタイムアウト指定をできるようにしました。

### Ver.1.2.8.0 2016/11/30
* exMXDRV 専用 API に対応。
* Unity から使用できるように参照アセンブリを最小限に整理した。
 * FMPMessageListener の実装を改め Windows Forms に依存しないようにした。

### Ver.1.2.7.0 2012/8/4
* FMPNote に Rest 定数を追加。
* FMPNote に Equals, GetHasCode を実装。

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
