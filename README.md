# AdaYazilim

# Tren Rezervasyon API

Tren ve vagon bilgilerine göre istenen kişi sayısı için rezervasyon uygunluğunu hesaplayan .NET 8 HTTP API uygulamasıdır. Vagonların %70 doluluk sınırını ve yolcuların farklı vagonlara yerleştirilme tercihini dikkate alarak yerleşim planı döndürür.

Uygulama gönderilen bilgiler üzerinden hesaplama yapar. Veritabanına rezervasyon kaydetmez veya sonraki istekler için koltuk doluluklarını güncellemez. Her istek bağımsız değerlendirilir.

## Canlı API

| Bilgi | Değer |
| --- | --- |
| Temel adres | `https://CANLI-API-ADRESI` |
| Rezervasyon endpoint'i | `POST https://CANLI-API-ADRESI/api/rezervasyon` |
| İstek formatı | `application/json` |

Rezervasyon endpoint'i POST isteği kabul eder. Adresi tarayıcının adres çubuğuna yazmak GET isteği gönderdiğinden rezervasyon hesabı başlatmaz. Kullanım için Postman veya aşağıdaki cURL örneği tercih edilebilir.

## Kullanılan Teknolojiler

| Teknoloji | Kullanım amacı |
| --- | --- |
| .NET 8 / ASP.NET Core Web API | Controller tabanlı HTTP API |
| FluentValidation | İstek verilerinin doğrulanması |
| AutoMapper | İstek DTO'larının modellere dönüştürülmesi |
| Swagger / OpenAPI | API sözleşmesinin gösterilmesi ve geliştirme ortamında manuel deneme |

## Yerel Ortamda Çalıştırma

### Gereksinimler

- .NET 8 SDK
- İsteğe bağlı olarak .NET 8 destekleyen Visual Studio ve ASP.NET/web geliştirme iş yükü

### Terminal ile

Repoyu indirdikten sonra `TrenRezervasyon.Api` klasörünün bulunduğu dizinde çalıştırın:

```bash
dotnet restore
dotnet run --project TrenRezervasyon.Api
```

Terminaldeki `Now listening on:` satırında uygulamanın adresi görünür. Aşağıdaki örneklerde `PORT` yerine bu adresteki port numarasını kullanın:

```text
https://localhost:PORT/api/rezervasyon
```

Uygulama HTTP adresinde çalışıyorsa aynı adresin `http` şemasını kullanın. Yerel adresler ve geliştirme profilleri `Properties/launchSettings.json` dosyasında yer alır.

### Visual Studio ile

1. Çözüm dosyasını açın.
2. `TrenRezervasyon.Api` projesini başlangıç projesi olarak seçin.
3. HTTPS profilini seçerek projeyi çalıştırın.
4. Swagger üzerinden veya Postman ile istek gönderin.

## API Kullanımı

```http
POST /api/rezervasyon
Content-Type: application/json
```

### İstek Alanları

| Alan | Tür | Açıklama |
| --- | --- | --- |
| `Tren` | Nesne | Rezervasyon uygunluğu hesaplanacak tren |
| `Tren.Ad` | String | Tren adı |
| `Tren.Vagonlar` | Dizi | Vagon bilgileri; en az bir vagon içermelidir |
| `Tren.Vagonlar[].Ad` | String | Vagon adı |
| `Tren.Vagonlar[].Kapasite` | Integer | Vagonun fiziksel koltuk kapasitesi; sıfırdan büyük olmalıdır |
| `Tren.Vagonlar[].DoluKoltukAdet` | Integer | Mevcut dolu koltuk sayısı; 0 ile kapasite arasında olmalıdır |
| `RezervasyonYapilacakKisiSayisi` | Integer | Sıfırdan büyük kişi sayısı |
| `KisilerFarkliVagonlaraYerlestirilebilir` | Boolean | `true`: grup bölünebilir; `false`: herkes aynı vagonda olmalıdır |

### Örnek İstek

```json
{
  "Tren": {
    "Ad": "Başkent Ekspres",
    "Vagonlar": [
      { "Ad": "Vagon 1", "Kapasite": 100, "DoluKoltukAdet": 68 },
      { "Ad": "Vagon 2", "Kapasite": 90, "DoluKoltukAdet": 50 },
      { "Ad": "Vagon 3", "Kapasite": 80, "DoluKoltukAdet": 80 }
    ]
  },
  "RezervasyonYapilacakKisiSayisi": 3,
  "KisilerFarkliVagonlaraYerlestirilebilir": true
}
```

### Başarılı Yerleşim — HTTP 200

```json
{
  "RezervasyonYapilabilir": true,
  "YerlesimAyrinti": [
    { "VagonAdi": "Vagon 1", "KisiSayisi": 2 },
    { "VagonAdi": "Vagon 2", "KisiSayisi": 1 }
  ]
}
```

İlk vagonun online rezervasyona uygun 2, ikinci vagonun 13 koltuğu vardır. Üçüncü vagon mevcut doluluğu nedeniyle yeni yerleşime uygun değildir.

Aynı istekte `KisilerFarkliVagonlaraYerlestirilebilir` alanı `false` olduğunda, üç kişinin tamamı ilk uygun vagon olan `Vagon 2` içine yerleştirilir.

### Yerleşim Yapılamaması — HTTP 200

Örnek istekte kişi sayısı `16` yapılırsa toplam kullanılabilir koltuk sayısı 15 olduğu için şu yanıt döner:

```json
{
  "RezervasyonYapilabilir": false,
  "YerlesimAyrinti": []
}
```

Bu durum bir girdi doğrulama hatası değildir. İstek başarıyla değerlendirilmiş, ancak uygun yerleşim bulunamamıştır. Bu nedenle HTTP 200 döner. Yolcuların yalnızca bir kısmı yerleştirilebiliyorsa da kısmi sonuç gönderilmez.

### Geçersiz Veri — HTTP 400

Negatif kişi sayısı, boş tren adı veya kapasiteden fazla dolu koltuk sayısı gibi geçersiz veriler HTTP 400 ile reddedilir. FluentValidation hataları `ValidationProblemDetails` içindeki `errors` alanında döndürülür.

Örneğin bir vagonun kapasitesi `100`, dolu koltuk sayısı `101` ise hata gövdesinde ilgili alanın doğrulama mesajı bulunur. Üst düzey başlık `Gönderilen bilgiler geçersiz.` olarak döner.

Geçersiz JSON veya sayısal alana `"abc"` gönderilmesi gibi dönüştürme hataları ise ASP.NET Core tarafından controller çalışmadan önce ele alınır. Bu mesajların biçimi ve dili FluentValidation mesajlarından farklı olabilir.

## Postman ile Kullanım

1. Yeni bir HTTP isteği oluşturun ve metodu **POST** seçin.
2. Canlı kullanım için `https://CANLI-API-ADRESI/api/rezervasyon` adresini yazın. Yerel kullanımda uygulamanın çalıştığı adresi kullanın.
3. **Body → raw → JSON** seçin.
4. Yukarıdaki örnek istek gövdesini yapıştırın.
5. `Content-Type` başlığının `application/json` olduğunu kontrol edin.
6. **Send** düğmesine basın.
7. HTTP durum kodunu ve yanıt gövdesini inceleyin.

## cURL ile Kullanım

Örnek istek JSON'unu UTF-8 kodlamasıyla `istek.json` adlı dosyaya kaydedin. Dosyanın bulunduğu dizinden çalıştırın:

```bash
curl --request POST "https://CANLI-API-ADRESI/api/rezervasyon" --header "Content-Type: application/json" --data-binary "@istek.json"
```

Windows PowerShell'de gerekirse `curl` yerine `curl.exe` kullanın. Yerel kullanımda URL'yi uygulamanızın yerel adresiyle değiştirin.



## Varsayımlar ve Kapsam

- Case metnindeki başarısız rezervasyon örneğinde bulunan `RezervasyonYapilabilir: true` değeri yazım hatası kabul edilmiştir. Uygun yerleşim bulunamadığında `false` döndürülür.
- Vagonlar gönderildikleri sırayla değerlendirilir; en az vagon kullanımı veya farklı bir optimizasyon hedeflenmez.
- API koltuk numarası seçmez; hangi vagona kaç kişi yerleştirilebileceğini hesaplar.
- Kalıcı rezervasyon kaydı ve eş zamanlı koltuk satışı kapsam dışındadır.
- İstemcilerin tüm istek alanlarını göndermesi beklenir. Mevcut DTO yapısında gönderilmeyen `bool` alan `false`, gönderilmeyen `int` alan `0` değerini alır. Bu nedenle eksik bölünme tercihi `false` kabul edilir; eksik dolu koltuk sayısı `0` olarak değerlendirilebilir.
