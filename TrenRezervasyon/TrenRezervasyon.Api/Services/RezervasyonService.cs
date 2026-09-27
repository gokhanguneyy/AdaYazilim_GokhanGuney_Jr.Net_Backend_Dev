using AutoMapper;
using TrenRezervasyon.Api.DTOs.Requests;
using TrenRezervasyon.Api.DTOs.Responses;
using TrenRezervasyon.Api.Models;

namespace TrenRezervasyon.Api.Services;

public class RezervasyonService : IRezervasyonService
{
    private readonly IMapper _mapper;

    public RezervasyonService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public RezervasyonResponseDto RezervasyonHesapla(RezervasyonRequestDto request)
    {
        var tren = _mapper.Map<Tren>(request.Tren);

        if (!request.KisilerFarkliVagonlaraYerlestirilebilir)
        {
            return AyniVagonaYerlestir(tren, request.RezervasyonYapilacakKisiSayisi);
        }

        return FarkliVagonlaraYerlestir(tren, request.RezervasyonYapilacakKisiSayisi);
    }

    private RezervasyonResponseDto AyniVagonaYerlestir(Tren tren, int kisiSayisi)
    {
        foreach (var vagon in tren.Vagonlar)
        {
            int kullanilabilirKoltukSayisi = vagon.KullanilabilirKoltukSayisiHesapla();

            if (kullanilabilirKoltukSayisi >= kisiSayisi)
            {
                return new RezervasyonResponseDto
                {
                    RezervasyonYapilabilir = true,
                    YerlesimAyrinti = new List<YerlesimAyrintiDto>
                    {
                        new YerlesimAyrintiDto
                        {
                            VagonAdi = vagon.Ad,
                            KisiSayisi = kisiSayisi
                        }
                    }
                };
            }
        }

        return new RezervasyonResponseDto
        {
            RezervasyonYapilabilir = false
        };
    }

    private RezervasyonResponseDto FarkliVagonlaraYerlestir(Tren tren, int kisiSayisi)
    {
        int kalanKisiSayisi = kisiSayisi;

        var yerlesimAyrinti = new List<YerlesimAyrintiDto>();

        foreach (var vagon in tren.Vagonlar)
        {
            int kullanilabilirKoltukSayisi = vagon.KullanilabilirKoltukSayisiHesapla();

            if (kullanilabilirKoltukSayisi == 0)
            {
                continue;
            }

            int yerlestirilecekKisiSayisi = Math.Min(kullanilabilirKoltukSayisi, kalanKisiSayisi);

            yerlesimAyrinti.Add(new YerlesimAyrintiDto
            {
                VagonAdi = vagon.Ad,
                KisiSayisi = yerlestirilecekKisiSayisi
            });

            kalanKisiSayisi -= yerlestirilecekKisiSayisi;

            if (kalanKisiSayisi == 0)
            {
                return new RezervasyonResponseDto
                {
                    RezervasyonYapilabilir = true,
                    YerlesimAyrinti = yerlesimAyrinti
                };
            }
        }

        return new RezervasyonResponseDto
        {
            RezervasyonYapilabilir = false
        };
    }
}