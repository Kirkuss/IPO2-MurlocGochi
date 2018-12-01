using Murloc_Tamagochi.Animaciones;
using Murloc_Tamagochi.Source.Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Murloc_Tamagochi
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Avatar miAvatar;
        private Estadisticas estadisticas;
        private DispatcherTimer temporizador;
        private Boolean muerto = false;
        private Boolean cambioTraje = false;
        private Boolean cambioFondo = false;

        private ReproductorSonido reproductor;
        private ReproductorSonido repMurloc;
        private ReproductorSonido repMusica;
        private ReproductorSonido repLogros;

        private ArrayList bolsas;
        private ArrayList logros;
        private ArrayList fondos;
        private ArrayList trajes;
        private ArrayList peces;

        private AnimacionSimple abrirMenuAnim, cerrarMenuAnim, respirarAnim, parpadearAnim, iniciarPartidaAnim, menuLogrosAbrir, menuLogrosCerrar, comer, dormir, jugar, bolsasAnim, abrirMuerteAnim, cerrarMuerteAnim, logroObtenido, menuTiendaAbrir, menuTiendaCerrar, cambiarTraje, pezAnim, ayudaAbrir, ayudaCerrar, click;

        public MainWindow()
        {
            InitializeComponent();

            reproductor = new ReproductorSonido();
            repMurloc = new ReproductorSonido();
            repMusica = new ReproductorSonido();
            repLogros = new ReproductorSonido();

            bolsas = new ArrayList();
            peces = new ArrayList();

            bolsasAnim = new AnimacionSimple((Storyboard)FindResource("Bolsa_1_caida"));
            bolsas.Add(bolsasAnim);
            bolsasAnim = new AnimacionSimple((Storyboard)FindResource("Bolsa_2_caida"));
            bolsas.Add(bolsasAnim);
            bolsasAnim = new AnimacionSimple((Storyboard)FindResource("Bolsa_3_caida"));
            bolsas.Add(bolsasAnim);
            pezAnim = new AnimacionSimple((Storyboard)FindResource("pez_1_mostrar"));
            peces.Add(pezAnim);
            pezAnim = new AnimacionSimple((Storyboard)FindResource("pez_2_mostrar"));
            peces.Add(pezAnim);
            pezAnim = new AnimacionSimple((Storyboard)FindResource("pez_3_mostrar"));
            peces.Add(pezAnim);

            click = new AnimacionSimple((Storyboard)FindResource("click"));
            comer = new AnimacionSimple((Storyboard)FindResource("Comer_anim"));
            dormir = new AnimacionSimple((Storyboard)FindResource("Dormir_anim"));
            jugar = new AnimacionSimple((Storyboard)FindResource("Saltos_anim"));
            abrirMenuAnim = new AnimacionSimple((Storyboard)FindResource("menu_Abrir"));
            cerrarMenuAnim = new AnimacionSimple((Storyboard)FindResource("menu_Cerrar"));
            respirarAnim = new AnimacionSimple((Storyboard)FindResource("Respirar_anim"));
            parpadearAnim = new AnimacionSimple((Storyboard)FindResource("Parpadeo_anim"));
            abrirMuerteAnim = new AnimacionSimple((Storyboard)FindResource("morir_Abrir"));
            cerrarMuerteAnim = new AnimacionSimple((Storyboard)FindResource("morir_Cerrar"));
            iniciarPartidaAnim = new AnimacionSimple((Storyboard)FindResource("pantallaInicial_Cerrar"));
            menuLogrosAbrir = new AnimacionSimple((Storyboard)FindResource("menuLogros_Abrir"));
            menuLogrosCerrar = new AnimacionSimple((Storyboard)FindResource("menuLogros_Cerrar"));
            logroObtenido = new AnimacionSimple((Storyboard)FindResource("logroConseguido"));
            menuTiendaAbrir = new AnimacionSimple((Storyboard)FindResource("menuTienda_Abrir"));
            menuTiendaCerrar = new AnimacionSimple((Storyboard)FindResource("menuTienda_Cerrar"));
            ayudaAbrir = new AnimacionSimple((Storyboard)FindResource("ayuda_Abrir"));
            ayudaCerrar = new AnimacionSimple((Storyboard)FindResource("ayuda_Cerrar"));

            repLogros.setVolumen(0.2);
            repMusica.setVolumen(0.1);
            repMusica.intro();

            gridPantallaInicial.Visibility = Visibility.Visible;

            temporizador = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            temporizador.Tick += tickConsumoHandler;
            temporizador.Start();
        }

        private void calcularExpMax()
        {
            if (miAvatar.Nivel == 0)
            {
                this.PB_Xp.Maximum = 100;
            }
            else
            {
                for(int i = 0; i<miAvatar.Nivel; i++)
                {
                    this.PB_Xp.Maximum += (int)(3 * (i + 1));
                }
            }
        }

        private void cargarInformacion()
        {
            miAvatar.Read();
            estadisticas = new Estadisticas(miAvatar.Nombre);
            estadisticas.Read();
            nombreJugadorlbl.Content = miAvatar.Nombre;
            iniciarPartidaAnim.Start();
            repMusica.juego();
            String path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Imagenes");
            ImageBrush img; 
            Grid imgCont;
            Label lbl;
            Rectangle rect;
            SolidColorBrush br = new SolidColorBrush(Colors.GreenYellow);
            Logro l = new Logro();
            Objeto o = new Objeto();
            logros = l.ReadAll();
            fondos = o.ReadAllFondos();
            trajes = o.ReadAllTrajes();
            for (int i = 0; i<logros.Count; i++)
            {
                l = (Logro)logros[i];
                img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(path + "\\" + l.Imagen));
                imgCont = (Grid)FindName("logro" + (i + 1) + "img");
                imgCont.Background = img;
                rect = (Rectangle)FindName("sombra" + (i + 1));
                if (l.Obtenido == 1) rect.Visibility = Visibility.Hidden;
            }
            for (int i = 0; i<fondos.Count; i++)
            {
                o = (Objeto)fondos[i];
                img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(path + "\\" + o.Imagen));
                imgCont = (Grid)FindName("fondo" + (i + 1) + "img");
                imgCont.Background = img;
                lbl = (Label)FindName("nombreFondo" + (i + 1) + "Lbl");
                lbl.Content = o.Nombre;
                lbl = (Label)FindName("costeFondo" + (i + 1) + "Lbl");
                if (o.Obtenido == 1)
                {
                    lbl.Content = "Adquirido";
                    lbl.Foreground = br;
                }
                else
                {
                    lbl.Content = "Req. nivel " + o.Nivel + " + " + o.Coste + " Oro";
                }
            }
            for (int i = 0; i < trajes.Count; i++)
            {
                o = (Objeto)trajes[i];
                img = new ImageBrush();
                img.ImageSource = new BitmapImage(new Uri(path + "\\" + o.Imagen));
                imgCont = (Grid)FindName("traje" + (i + 1) + "img");
                imgCont.Background = img;
                lbl = (Label)FindName("nombreTraje" + (i + 1) + "Lbl");
                lbl.Content = o.Nombre;
                lbl = (Label)FindName("costeTraje" + (i + 1) + "Lbl");
                if (o.Obtenido == 1)
                {
                    lbl.Content = "Adquirido";
                    lbl.Foreground = br;
                }
                else
                {
                    lbl.Content = "Req. nivel " + o.Nivel + " + " + o.Coste + " Oro";
                }
            }        
            cambiarTraje = new AnimacionSimple((Storyboard)FindResource("traje_" + miAvatar.Traje));
            cambiarTraje.Start();
            img = new ImageBrush();
            img.ImageSource = new BitmapImage(new Uri(path + "\\" + miAvatar.Paisaje));
            gridPantallaAvatar.Background = img;
            calcularExpMax();
            respirarAnim.Start();
            parpadearAnim.Start();
        }

        private int getNum(int max)
        {
            Random rnd = new Random();
            return rnd.Next(0, max);
        }

        private void tickConsumoHandler(object sender, EventArgs e)
        {
            if (miAvatar != null && estadisticas != null)
            {
                estadisticas.TiempoAvatar += 1;

                if (miAvatar.Apetito >= 100) miAvatar.Apetito = 100;
                if (miAvatar.Energia >= 100) miAvatar.Energia = 100;
                if (miAvatar.Diversion >= 100) miAvatar.Diversion = 100;
                miAvatar.Apetito = miAvatar.Apetito - 2;
                miAvatar.Diversion = miAvatar.Diversion - 2;
                miAvatar.Energia = miAvatar.Energia - 2;
                this.PB_Apetito.Value = miAvatar.Apetito;
                this.PB_Diversion.Value = miAvatar.Diversion;
                this.PB_Energia.Value = miAvatar.Energia;
                miAvatar.Experiencia += 5 + (int)(1.5 * miAvatar.Nivel);
                miAvatar.Oro += 3 + (int)(0.5 * miAvatar.Nivel);
                Nivel_txt.Text = Convert.ToString(miAvatar.Nivel);

                if (estadisticas.TiempoAvatar % 5 == 0)
                {
                    bolsasAnim = (AnimacionSimple)bolsas[getNum(3)];
                    bolsasAnim.Start();
                    reproductor.Golpe();
                }
                if (estadisticas.TiempoAvatar % 15 == 0)
                {
                    pezAnim = (AnimacionSimple)peces[getNum(3)];
                    pezAnim.Start();
                }
                if (miAvatar.Experiencia >= this.PB_Xp.Maximum)
                {
                    miAvatar.Experiencia = 0;
                    this.PB_Xp.Value = 0;
                    miAvatar.Nivel += 1;
                    this.PB_Xp.Maximum += (int)(3 * miAvatar.Nivel);
                }
                this.PB_Xp.Value = miAvatar.Experiencia;
                currentXP_txt.Text = Convert.ToString(miAvatar.Experiencia);
                currentOro_txt.Text = Convert.ToString(miAvatar.Oro);
                maxXP_txt.Text = "/" + Convert.ToString(this.PB_Xp.Maximum);
                miAvatar.update();
                estadisticas.update();
                comprobarLogros();
                if (Muerte())
                {
                    Morir();
                }
            }
        }

        private void BT_Comer_Click_1(object sender, RoutedEventArgs e)
        {
            SolidColorBrush br = new SolidColorBrush(Colors.Green);
            if (animacionEnCurso())
            {
                info_Ap_btt.Text = "+3";
                info_Di_btt.Text = "";
                info_En_btt.Text = "";
                info_Ap_btt.Foreground = br;
                pararAnimaciones();
                comer.Animacion.Completed += reanudarAnimaciones;
                repMurloc.comer();
                click.Start();
                comer.Start();
            }
            miAvatar.Apetito += 3;
            this.PB_Apetito.Value = miAvatar.Apetito;
            estadisticas.ClicksComer += 1;
        }

        private Boolean animacionEnCurso()
        {
            if(respirarAnim.Reproduciendo || parpadearAnim.Reproduciendo || comer.Reproduciendo || dormir.Reproduciendo || jugar.Reproduciendo)
            {
                return true;
            }
            return false;
        }

        private void pararAnimaciones()
        {
            respirarAnim.Stop();
            parpadearAnim.Stop();
            comer.Stop();
            dormir.Stop();
            jugar.Stop();
        }

        private void reanudarAnimaciones(object sender, EventArgs e)
        {
            respirarAnim.Start();
            parpadearAnim.Start();
        }

        private void BT_Jugar_Click_1(object sender, RoutedEventArgs e)
        {
            SolidColorBrush red = new SolidColorBrush(Colors.Red);
            SolidColorBrush br = new SolidColorBrush(Colors.Green);
            if (animacionEnCurso())
            {
                info_Ap_btt.Text = "-1";
                info_Di_btt.Text = "+3";
                info_En_btt.Text = "-2";
                info_Ap_btt.Foreground = red;
                info_En_btt.Foreground = red;
                info_Di_btt.Foreground = br;
                pararAnimaciones();
                jugar.Animacion.Completed += reanudarAnimaciones;
                repMurloc.saludar();
                click.Start();
                jugar.Start();
            }
            miAvatar.Diversion += 3;
            this.PB_Diversion.Value = miAvatar.Diversion;
            miAvatar.Energia -= 2;
            this.PB_Energia.Value = miAvatar.Energia;
            miAvatar.Apetito -= 1;
            this.PB_Apetito.Value = miAvatar.Apetito;
            estadisticas.ClickJugar += 1;
        }

        private void logroInfo_Click(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            String nombre = Regex.Match(g.Name, @"\d+").Value;
            int i = Int32.Parse(nombre);
            Logro l = (Logro)logros[i - 1];
            nombreLogroLbl.Content = l.Nombre;
            descLogroLbl.Content = l.Descripcion;
        }

        private void nuevaPartida_Click(object sender, RoutedEventArgs e)
        {
                miAvatar = new Avatar();
                if (miAvatar.count() == 0)
                {
                    miAvatar = new Avatar(100, 100, 100);
                    miAvatar.Nombre = Convert.ToString(nombreInicialTxt.Text);
                    miAvatar.Experiencia = 0;
                    miAvatar.Nivel = 0;
                    miAvatar.Oro = 0;
                    miAvatar.Traje = 2;
                    miAvatar.Paisaje = "Fondo_1.jpg";
                    miAvatar.insert();
                    estadisticas = new Estadisticas(miAvatar.Nombre);
                    estadisticas.insert();
                    nombreJugadorlbl.Content = miAvatar.Nombre;
                    iniciarPartidaAnim.Start();
                    ReiniciarDatos();
                }
                else
                {
                    estadisticas = new Estadisticas();
                    miAvatar.delete();
                    estadisticas.delete();
                    miAvatar = new Avatar(100, 100, 100);
                    miAvatar.Nombre = Convert.ToString(nombreInicialTxt.Text);
                    miAvatar.Experiencia = 0;
                    miAvatar.Nivel = 0;
                    miAvatar.Oro = 0;
                    miAvatar.Traje = 2;
                    miAvatar.Paisaje = "Fondo_1.jpg";
                    miAvatar.insert();
                    estadisticas = new Estadisticas(miAvatar.Nombre);
                    estadisticas.insert();
                    nombreJugadorlbl.Content = miAvatar.Nombre;
                    iniciarPartidaAnim.Start();
                    ReiniciarDatos();
                }
            try
            {
                cargarInformacion();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            repMusica.juego();
        }

        private void continuar_Click(object sender, RoutedEventArgs e)
        {
            miAvatar = new Avatar();
            if (miAvatar.count() == 0)
            {
                MessageBox.Show("¡Vaya! parece que ese no es el murloc que estabas buscando, si es la primera vez que juegas, ponle nombre e inicia una nueva partida.");
                miAvatar = null;
            }
            else
            {
                try
                {
                    cargarInformacion();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } 
            }
        }

        private void tocar_Click(object sender, MouseButtonEventArgs e)
        {
            int aux = getNum(4);
            repMurloc.sonidoAleatorio(aux);
        }

        private void silencio_Ev(object sender, RoutedEventArgs e)
        {
            repMurloc.setVolumen(0);
            repMusica.setVolumen(0);
            reproductor.setVolumen(0);
            repLogros.setVolumen(0);
        }

        private void sonido_Ev(object sender, RoutedEventArgs e)
        {
            repMurloc.setVolumen(0.5);
            repMusica.setVolumen(0.1);
            reproductor.setVolumen(0.5);
            repLogros.setVolumen(0.2);
        }

        private void salirMuerto_Click(object sender, RoutedEventArgs e)
        {
            miAvatar.delete();
            estadisticas.delete();
            ReiniciarDatos();
            this.Close();
        }

        private void logros_Click(object sender, RoutedEventArgs e)
        {
            if (!muerto)
            {
                if (gridMenuLogros.Visibility == Visibility.Hidden)
                {
                    if (menuAbierto()) { cerrarMenus("logros"); }
                    menuLogrosAbrir.Start();
                    logrosBt.Content = "Cerrar logros";
                    Pausar();
                }
                else
                {
                    menuLogrosCerrar.Start();
                    logrosBt.Content = "Abrir logros";
                    Continuar();
                }
            } 
        }

        private void comprobarLogros()
        {
            Logro l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, l16, l17, l18, l19, l20, l21, l22, l23, l24, l25;

            l1 = (Logro)logros[0];
            l2 = (Logro)logros[1];
            l3 = (Logro)logros[2];
            l4 = (Logro)logros[3];
            l5 = (Logro)logros[4];
            l6 = (Logro)logros[5];
            l7 = (Logro)logros[6];
            l8 = (Logro)logros[7];
            l9 = (Logro)logros[8];
            l10 = (Logro)logros[9];
            l11 = (Logro)logros[10];
            l12 = (Logro)logros[11];
            l13 = (Logro)logros[12];
            l14 = (Logro)logros[13];
            l15 = (Logro)logros[14];
            l16 = (Logro)logros[15];
            l17 = (Logro)logros[16];
            l18 = (Logro)logros[17];
            l19 = (Logro)logros[18];
            l20 = (Logro)logros[19];
            l21 = (Logro)logros[20];
            l22 = (Logro)logros[21];
            l23 = (Logro)logros[22];
            l24 = (Logro)logros[23];
            l25 = (Logro)logros[24];

            if ((estadisticas.ClicksComer + estadisticas.ClicksDormir + estadisticas.ClickJugar + estadisticas.ClicksBolsas + estadisticas.ClicksPeces) >= 1 && l1.Obtenido != 1) { repLogros.logro(); cargarLogro(l1); }
            else if (miAvatar.Nivel >= 5 && l2.Obtenido != 1) { repLogros.logro(); cargarLogro(l2); }
            else if (miAvatar.Nivel >= 10 && l3.Obtenido != 1) { repLogros.logro(); cargarLogro(l3); }
            else if (miAvatar.Nivel >= 20 && l4.Obtenido != 1) { repLogros.logro(); cargarLogro(l4); }
            else if (miAvatar.Nivel >= 30 && l5.Obtenido != 1) { repLogros.logro(); cargarLogro(l5); }
            else if (miAvatar.Nivel >= 60 && l6.Obtenido != 1) { repLogros.logro(); cargarLogro(l6); }
            else if (miAvatar.Oro >= 100 && l7.Obtenido != 1) { repLogros.logro(); cargarLogro(l7); }
            else if (miAvatar.Oro >= 250 && l8.Obtenido != 1) { repLogros.logro(); cargarLogro(l8); }
            else if (miAvatar.Oro >= 500 && l9.Obtenido != 1) { repLogros.logro(); cargarLogro(l9); }
            else if (miAvatar.Oro >= 1000 && l10.Obtenido != 1) { repLogros.logro(); cargarLogro(l10); }
            else if (estadisticas.ClicksBolsas >= 10 && l11.Obtenido != 1) { repLogros.logro(); cargarLogro(l11); }
            else if (estadisticas.ClicksBolsas >= 20 && l12.Obtenido != 1) { repLogros.logro(); cargarLogro(l12); }
            else if (estadisticas.ClicksPeces >= 10 && l13.Obtenido != 1) { repLogros.logro(); cargarLogro(l13); }
            else if (estadisticas.ClicksPeces >= 20 && l14.Obtenido != 1) { repLogros.logro(); cargarLogro(l14); }
            else if (estadisticas.ClicksComer >= 200 && l15.Obtenido != 1) { repLogros.logro(); cargarLogro(l15); }
            else if (estadisticas.ClicksComer >= 600 && l16.Obtenido != 1) { repLogros.logro(); cargarLogro(l16); }
            else if (estadisticas.ClicksDormir >= 200 && l17.Obtenido != 1) { repLogros.logro(); cargarLogro(l17); }
            else if (estadisticas.ClicksDormir >= 600 && l18.Obtenido != 1) { repLogros.logro(); cargarLogro(l18); }
            else if (estadisticas.ClickJugar >= 200 && l19.Obtenido != 1) { repLogros.logro(); cargarLogro(l19); }
            else if (estadisticas.ClickJugar >= 600 && l20.Obtenido != 1) { repLogros.logro(); cargarLogro(l20); }
            else if (cambioTraje && l21.Obtenido != 1) { repLogros.logro(); cargarLogro(l21); }
            else if (cambioFondo && l22.Obtenido != 1) { repLogros.logro(); cargarLogro(l22); }
            else if (l23.completados(logros, l23)) { repLogros.logro(); cargarLogro(l23); }
            else if (estadisticas.TiempoAvatar >= 300) { repLogros.logro(); cargarLogro(l24); }
            else if (estadisticas.TiempoAvatar >= 600) { repLogros.logro(); cargarLogro(l25); }
        }

        private void ayuda_Click(object sender, RoutedEventArgs e)
        {
            if (!muerto)
            {
                if (gridAyuda.Visibility == Visibility.Hidden)
                {
                    if (menuAbierto()) { cerrarMenus("ayuda"); }
                    ayudaAbrir.Start();
                    ayudaBt.Content = "Cerrar ayuda";
                    Pausar();
                }
                else
                {
                    ayudaCerrar.Start();
                    ayudaBt.Content = "Abrir ayuda";
                    Continuar();
                }
            }
        }

        private void pez_Click(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush br = new SolidColorBrush(Colors.Green);
            info_Ap_btt.Text = "+40";
            info_Di_btt.Text = "+25";
            info_En_btt.Text = "";
            info_Ap_btt.Foreground = br;
            info_Di_btt.Foreground = br;
            miAvatar.Apetito += 40;
            miAvatar.Diversion += 25;
            miAvatar.Experiencia += 50;
            estadisticas.ClicksPeces += 1;
            click.Start();
            repMurloc.comer();
            pezAnim.Stop();
        }

        private void avisoLogro_Click(object sender, MouseButtonEventArgs e)
        {
            logroObtenido.Stop();
        }

        private void comprarClick(object sender, MouseButtonEventArgs e)
        {
            String num;
            Objeto o;
            Label lbl;
            Grid selected = (Grid)sender;
            SolidColorBrush br = new SolidColorBrush(Colors.GreenYellow);
            String path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Imagenes");
            ImageBrush img = new ImageBrush();
            if (selected.Name.Contains("fondo"))
            {
                num = Regex.Match(selected.Name, @"\d+").Value;
                o = (Objeto)fondos[Int32.Parse(num) - 1];
                if (o.Obtenido != 1)
                {
                    if (o.comprar(miAvatar))
                    {
                        lbl = (Label)FindName("costeFondo" + num + "Lbl");
                        lbl.Content = "Adquirido";
                        lbl.Foreground = br;
                        img.ImageSource = new BitmapImage(new Uri(path + "\\" + o.Imagen));
                        gridPantallaAvatar.Background = img;
                        miAvatar.Paisaje = o.Imagen;
                        cambioFondo = true;
                        reproductor.comprar();
                        repMurloc.confuso();
                    }
                    else
                    {
                        MessageBox.Show("¡No cumples los requisitos para poder mudarte aqui!");
                    }
                }
                else
                {
                    img.ImageSource = new BitmapImage(new Uri(path + "\\" + o.Imagen));
                    gridPantallaAvatar.Background = img;
                    miAvatar.Paisaje = o.Imagen;
                    repMurloc.confuso();
                }
            }
            else
            {
                num = Regex.Match(selected.Name, @"\d+").Value;
                o = (Objeto)trajes[Int32.Parse(num) - 1];
                if (o.Obtenido != 1)
                {
                    if (o.comprar(miAvatar))
                    {
                        lbl = (Label)FindName("costeTraje" + num + "Lbl");
                        lbl.Content = "Adquirido";
                        lbl.Foreground = br;
                        cambiarTraje = new AnimacionSimple((Storyboard)FindResource("traje_" + num));
                        cambiarTraje.Start();
                        miAvatar.Traje = Int32.Parse(num);
                        cambioTraje = true;
                        reproductor.comprar();
                        repMurloc.confuso();
                    }
                    else
                    {
                        MessageBox.Show("¡No cumples los requisitos para poder comprar este traje!");
                    }
                }
                else
                {
                    cambiarTraje = new AnimacionSimple((Storyboard)FindResource("traje_" + num));
                    cambiarTraje.Start();
                    miAvatar.Traje = Int32.Parse(num);
                    repMurloc.confuso();
                }
            }
        }

        private void tienda_Click(object sender, RoutedEventArgs e)
        {
            if (!muerto) { 
                if (gridMenuTienda.Visibility == Visibility.Hidden)
                {
                    if (menuAbierto()) { cerrarMenus("tienda"); }
                    tiendaBt.Content = "Cerrar tienda";
                     menuTiendaAbrir.Start();
                    Pausar();
                }
                else
                {
                    menuTiendaCerrar.Start();
                    tiendaBt.Content = "Abrir tienda";
                    Continuar();
                }
            }
        }

        public void ReiniciarDatos()
        {
            try
            {
                Logro l = new Logro();
                Objeto o = new Objeto();
                l.reset();
                o.reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cargarLogro(Logro l)
        {
            String path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Imagenes");
            ImageBrush img = new ImageBrush();
            Rectangle r;
            switch (l.Tipo)
            {
                case 0:
                    expObtenidaLbl.Content = "+" + l.Recompensa;
                    oroObtenidoLbl.Content = "";
                    break;
                case 1:
                    expObtenidaLbl.Content = "";
                    oroObtenidoLbl.Content = "+" + l.Recompensa;
                    break;
                case 2:
                    expObtenidaLbl.Content = "+" + l.Recompensa;
                    oroObtenidoLbl.Content = "+" + l.Recompensa;
                    break;
            }
            r = (Rectangle)FindName("sombra" + l.IdLogro);
            r.Visibility = Visibility.Hidden;
            img.ImageSource = new BitmapImage(new Uri(path + "\\" + l.Imagen));
            logroObtImg.Background = img;
            nombreLogroConseguidoLbl.Content = l.Nombre;
            l.Recompensar(miAvatar);
            logroObtenido.Start();
        }

        private Boolean menuAbierto()
        {
            if (gridMenuLogros.Visibility == Visibility.Visible || gridMenuPrincipal.Visibility == Visibility.Visible || gridMenuTienda.Visibility == Visibility.Visible || gridAyuda.Visibility == Visibility.Visible) {
                return true;
            }
            return false;
        }

        private void cerrarMenus(String abriendo)
        {
            switch (abriendo)
            {
                case "menu":
                    if (gridMenuLogros.Visibility == Visibility.Visible) menuLogrosCerrar.Start();
                    if (gridMenuTienda.Visibility == Visibility.Visible) menuTiendaCerrar.Start();
                    if (gridAyuda.Visibility == Visibility.Visible) ayudaCerrar.Start();
                    logrosBt.Content = "Abrir logros";
                    tiendaBt.Content = "Abrir tienda";
                    ayudaBt.Content = "Abrir ayuda";
                    break;
                case "logros":
                    if (gridMenuPrincipal.Visibility == Visibility.Visible) cerrarMenuAnim.Start();
                    if (gridMenuTienda.Visibility == Visibility.Visible) menuTiendaCerrar.Start();
                    if (gridAyuda.Visibility == Visibility.Visible) ayudaCerrar.Start();
                    tiendaBt.Content = "Abrir tienda";
                    ayudaBt.Content = "Abrir ayuda";
                    menuBt.Content = "Abrir menú";
                    break;
                case "tienda":
                    if (gridMenuLogros.Visibility == Visibility.Visible) menuLogrosCerrar.Start();
                    if (gridAyuda.Visibility == Visibility.Visible) ayudaCerrar.Start();
                    if (gridMenuPrincipal.Visibility == Visibility.Visible) cerrarMenuAnim.Start();
                    menuBt.Content = "Abrir menú";
                    logrosBt.Content = "Abrir logros";
                    ayudaBt.Content = "Abrir ayuda";
                    break;
                case "ayuda":
                    if (gridMenuLogros.Visibility == Visibility.Visible) menuLogrosCerrar.Start();
                    if (gridMenuPrincipal.Visibility == Visibility.Visible) cerrarMenuAnim.Start();
                    if (gridMenuTienda.Visibility == Visibility.Visible) menuTiendaCerrar.Start();
                    menuBt.Content = "Abrir menú";
                    logrosBt.Content = "Abrir logros";
                    tiendaBt.Content = "Abrir tienda";
                    break;
            }         
        }

        private void BT_Dormir_Click_1(object sender, RoutedEventArgs e)
        {
            SolidColorBrush br = new SolidColorBrush(Colors.Green);
            if (animacionEnCurso())
            {
                info_Ap_btt.Text = "";
                info_Di_btt.Text = "";
                info_En_btt.Text = "+3";
                info_En_btt.Foreground = br;
                pararAnimaciones();
                dormir.Animacion.Completed += reanudarAnimaciones;
                repMurloc.roncar();
                click.Start();
                dormir.Start();
            }
            miAvatar.Energia += 3;
            this.PB_Energia.Value = miAvatar.Energia;
            estadisticas.ClicksDormir += 1;
        }

        private bool Muerte()
        {
            if (miAvatar.Energia <= 0 || miAvatar.Apetito <= 0 || miAvatar.Diversion <= 0)
            {
                return true;
            }
            return false;
        }

        private void Morir()
        {
            gridAvatar.Visibility = Visibility.Hidden;
            gridAvatarMuerto.Visibility = Visibility.Visible;
            tumbaLbl.Content = miAvatar.Nombre;
            repMurloc.morir();
            abrirMuerteAnim.Start();
            Pausar();
            muerto = true;
        }

        private void Bolsa_Click(object sender, MouseButtonEventArgs e)
        {
            reproductor.coger();
            bolsasAnim.Stop();
            estadisticas.ClicksBolsas += 1;
            miAvatar.Oro += 25;
            currentOro_txt.Text = Convert.ToString(miAvatar.Oro);
        }

        private void abrirMenu_Click(object sender, RoutedEventArgs e)
        {
            if (!muerto)
            {
                if (gridMenuPrincipal.Visibility == Visibility.Hidden)
                {
                    if (menuAbierto()) { cerrarMenus("menu"); }
                    abrirMenuAnim.Start();
                    ActualizarMenu();
                    menuBt.Content = "Cerrar menú";
                    Pausar();
                }
                else
                {
                    cerrarMenuAnim.Start();
                    menuBt.Content = "Abrir menú";
                    Continuar();
                }
            }
        }

        private void reiniciar_Click(object sender, RoutedEventArgs e)
        {
            miAvatar.delete();
            estadisticas.delete();
            miAvatar = new Avatar(100, 100, 100);
            miAvatar.Nombre = Convert.ToString(nombreJugadorlbl.Content);
            miAvatar.Experiencia = 0;
            miAvatar.Nivel = 0;
            miAvatar.Oro = 0;
            miAvatar.Traje = 2;
            miAvatar.Paisaje = "Fondo_1.jpg";
            miAvatar.insert();
            ReiniciarDatos();
            estadisticas = new Estadisticas(miAvatar.Nombre);
            estadisticas.insert();
            try
            {
                cargarInformacion();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ActualizarMenu();
        }

        private void Pausar()
        {
            temporizador.Stop();
        }

        private void Continuar()
        { 
            temporizador.Start();
        }

        private void ActualizarMenu()
        {
            int num, hor, min, seg;
            num = Convert.ToInt32(estadisticas.TiempoAvatar);
            hor = (int)(num / 3600);
            min = (int)((num - hor * 3600) / 60);
            seg = num - (int)(hor * 3600 + min * 60);
            nombreEstLbl.Content = estadisticas.Avatar;
            tjugadoEstLbl.Content = hor + " : " + min + " : " + seg;
            ClicksJEstLbl.Content = Convert.ToInt32(estadisticas.ClickJugar);
            ClicksDEstLbl.Content = Convert.ToInt32(estadisticas.ClicksDormir);
            ClicksCEstLbl.Content = Convert.ToInt32(estadisticas.ClicksComer);
            bolsasEstLbl.Content = Convert.ToInt32(estadisticas.ClicksBolsas);
            pecesEstLbl.Content = Convert.ToInt32(estadisticas.ClicksPeces);
        }
    }
}
