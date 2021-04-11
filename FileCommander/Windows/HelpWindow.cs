using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class HelpWindow : Window
    {
        public Button YesButton { get; set; }

        public Button OkButton { get; set;}


        const string DEFAULT_NAME = "Help";

        private string _text = "Равным образом консультация с профессионалами из IT способствует подготовке и реализации существующих финансовых и административных условий? С другой стороны начало повседневной работы по формированию позиции способствует подготовке и реализации всесторонне сбалансированных нововведений!\nС другой стороны начало повседневной работы по формированию позиции способствует подготовке и реализации новых предложений.\r" +
            "Задача организации, в особенности же дальнейшее развитие различных форм деятельности обеспечивает актуальность направлений прогрессивного развития.Дорогие друзья, постоянный количественный рост и сфера нашей активности в значительной степени обуславливает создание направлений прогрессивного развития? Таким образом, консультация с профессионалами из IT способствует подготовке и реализации ключевых компонентов планируемого обновления? Повседневная практика показывает, что постоянный количественный рост и сфера нашей активности обеспечивает широкому кругу специалистов участие в формировании ключевых компонентов планируемого обновления.\r" +
            "С другой стороны новая модель организационной деятельности требует от нас системного анализа форм воздействия? Задача организации, в особенности же рамки и место обучения кадров играет важную роль в формировании экономической целесообразности принимаемых решений.Повседневная практика показывает, что рамки и место обучения кадров в значительной степени обуславливает создание модели развития! Значимость этих проблем настолько очевидна, что повышение уровня гражданского сознания обеспечивает широкому кругу специалистов участие в формировании дальнейших направлений развитая системы массового участия.\r" +
            "С другой стороны выбранный нами инновационный путь обеспечивает широкому кругу специалистов участие...";


        public HelpWindow(Size targetSize, string message) : base("50%-38, 50%-12, 75, 22", targetSize)
        {
            Name = DEFAULT_NAME;
            Footer = "ESC to close window";
            ForegroundColor = Theme.HelpWindowForegroundColor;
            BackgroundColor = Theme.HelpWindowBackgroundColor;
            var label = new Label("1, 1, 100%-2, 100%-2", Size, Alignment.None,  "HelpText", _text);
            label.TextAlignment = TextAlignment.Width;
            label.Wrap = true;
            Add(label);
        }

        private void AddButtons()
        {

            OkButton = new Button("32,100%-2, 10, 1", Size, Alignment.None, "Ok")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Cancel
            };
            Add(OkButton);

            SetFocus(YesButton, false);
        }


        //public override void Draw(Buffer buffer, int targetX, int targetY)
        //{
        //    base.Draw(buffer, targetX, targetY);
        //    var line = new Line(X, Y + Height - 3, Width, 1, Direction.Horizontal, LineType.Single);
        //    line.FirstChar = '╟';
        //    line.LastChar = '╢';
        //    line.ForegroundColor = ForegroundColor;
        //    line.BackgroundColor = BackgroundColor;
        //    line.Draw(buffer, targetX, targetY);
        //}
    }
}