using System;
using System.Collections.Generic;

class Program
{
    class Urun
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public int Stok { get; set; }
        public decimal Fiyat { get; set; } // Satış fiyatı
        public decimal AlisFiyati { get; set; } // Toptan alış fiyatı
        public int SatilanMiktar { get; set; } = 0; // Satış miktarı
        public decimal ToplamKar { get; set; } = 0; // Bu üründen elde edilen toplam kâr
    }

    static List<Urun> urunListesi = new List<Urun>();
    static int urunIdSayac = 1;
    static decimal bakiye = 1000m; // Kullanıcının başlangıç bakiyesi

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n--- Satış Deposu Sistemi ---");
            Console.WriteLine($"Mevcut Bakiye: {bakiye:C}");
            Console.WriteLine("1. Ürün Ekle");
            Console.WriteLine("2. Stok Görüntüle");
            Console.WriteLine("3. Satış Yap");
            Console.WriteLine("4. Gün Sonu Raporu");
            Console.WriteLine("5. Çıkış");
            Console.Write("Seçiminizi yapın: ");
            string secim = Console.ReadLine();

            if (secim == "1")
                UrunIslemleri();
            else if (secim == "2")
                StokGoruntule();
            else if (secim == "3")
                SatisYap();
            else if (secim == "4")
                GunSonuRaporu();
            else if (secim == "5")
            {
                Console.WriteLine("Programdan çıkılıyor...");
                break;
            }
            else
                Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
        }
    }

    static void UrunIslemleri()
    {
        Console.WriteLine("\n--- Ürün Listesi ---");
        foreach (var urun in urunListesi)
        {
            Console.WriteLine($"ID: {urun.Id}, Ad: {urun.Ad}, Stok: {urun.Stok}, Satış Fiyatı: {urun.Fiyat:C}, Alış Fiyatı: {urun.AlisFiyati:C}");
        }

        if (urunListesi.Count == 0)
            Console.WriteLine("Henüz ürün eklenmemiş.");

        Console.WriteLine("\n1. Yeni Ürün Ekle");
        Console.WriteLine("2. Mevcut Ürünün Stok Miktarını Artır");
        Console.Write("Seçiminizi yapın: ");
        string secim = Console.ReadLine();

        if (secim == "1")
        {
            YeniUrunEkle();
        }
        else if (secim == "2")
        {
            MevcutUrunStokArttir();
        }
        else
        {
            Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
        }
    }

    static void YeniUrunEkle()
    {
        Console.Write("Ürün Adı: ");
        string ad = Console.ReadLine();

        Console.Write("Stok Miktarı: ");
        int stok = int.Parse(Console.ReadLine());

        Console.Write("Toptan Alış Fiyatı: ");
        decimal alisFiyati = decimal.Parse(Console.ReadLine());

        Console.Write("Satış Fiyatı: ");
        decimal fiyat = decimal.Parse(Console.ReadLine());

        Urun yeniUrun = new Urun
        {
            Id = urunIdSayac++,
            Ad = ad,
            Stok = stok,
            Fiyat = fiyat,
            AlisFiyati = alisFiyati
        };

        urunListesi.Add(yeniUrun);
        Console.WriteLine("Yeni ürün başarıyla eklendi!");
    }

    static void MevcutUrunStokArttir()
    {
        Console.Write("Stok miktarını artırmak istediğiniz ürünün ID'sini girin: ");
        int id = int.Parse(Console.ReadLine());

        Urun urun = urunListesi.Find(u => u.Id == id);
        if (urun != null)
        {
            Console.Write($"Mevcut stok: {urun.Stok}. Eklemek istediğiniz miktar: ");
            int miktar = int.Parse(Console.ReadLine());

            decimal maliyet = miktar * urun.AlisFiyati;

            if (maliyet > bakiye)
            {
                Console.WriteLine($"Bakiye yetersiz! Gerekli Tutar: {maliyet:C}, Mevcut Bakiye: {bakiye:C}");
            }
            else
            {
                urun.Stok += miktar;
                bakiye -= maliyet;
                Console.WriteLine($"Stok başarıyla artırıldı! Yeni stok: {urun.Stok}. Harcanan Tutar: {maliyet:C}");
            }
        }
        else
        {
            Console.WriteLine("Girilen ID'ye ait ürün bulunamadı.");
        }
    }

    static void StokGoruntule()
    {
        Console.WriteLine("\n--- Depodaki Ürünler ---");
        foreach (var urun in urunListesi)
        {
            Console.WriteLine($"ID: {urun.Id}, Ad: {urun.Ad}, Stok: {urun.Stok}, Satış Fiyatı: {urun.Fiyat:C}, Alış Fiyatı: {urun.AlisFiyati:C}");
        }

        if (urunListesi.Count == 0)
            Console.WriteLine("Depoda hiç ürün yok.");
    }

    static void SatisYap()
    {
        if (urunListesi.Count == 0)
        {
            Console.WriteLine("Henüz ürün eklenmemiş. Satış yapılamaz.");
            return;
        }

        Console.WriteLine("\n--- Satış Menüsü ---");
        foreach (var Urun in urunListesi)
        {
            Console.WriteLine($"ID: {Urun.Id}, Ad: {Urun.Ad}, Stok: {Urun.Stok}, Fiyat: {Urun.Fiyat:C}");
        }

        Console.Write("Satılacak Ürün ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Satılacak Miktar: ");
        int miktar = int.Parse(Console.ReadLine());

        Urun urun = urunListesi.Find(u => u.Id == id);
        if (urun != null)
        {
            if (urun.Stok >= miktar)
            {
                decimal toplamFiyat = miktar * urun.Fiyat;

                urun.Stok -= miktar;
                urun.SatilanMiktar += miktar;

                decimal toplamAlisMaliyeti = miktar * urun.AlisFiyati;
                decimal kar = toplamFiyat - toplamAlisMaliyeti;

                urun.ToplamKar += kar;
                bakiye += toplamFiyat;

                Console.WriteLine($"Satış başarılı! Toplam Tutar: {toplamFiyat:C}, Bu satıştan elde edilen kâr: {kar:C}, Güncel Bakiye: {bakiye:C}");
            }
            else
            {
                Console.WriteLine("Yeterli stok yok!");
            }
        }
        else
        {
            Console.WriteLine("Ürün bulunamadı!");
        }
    }

    static void GunSonuRaporu()
    {
        Console.WriteLine("\n--- Gün Sonu Raporu ---");
        decimal genelKar = 0;

        foreach (var urun in urunListesi)
        {
            if (urun.SatilanMiktar > 0)
            {
                Console.WriteLine($"Ürün: {urun.Ad}");
                Console.WriteLine($"Satılan Miktar: {urun.SatilanMiktar}");
                Console.WriteLine($"Bu Üründen Toplam Kâr: {urun.ToplamKar:C}");
                Console.WriteLine("----------------------------------");
                genelKar += urun.ToplamKar;
            }
        }

        Console.WriteLine($"Genel Toplam Kâr: {genelKar:C}");
        Console.WriteLine($"Gün Sonu Bakiye: {bakiye:C}");

        if (genelKar == 0)
            Console.WriteLine("Bugün satış yapılmadı.");
    }
}
