using Microsoft.Maui.Graphics;

namespace CMM.Mobile.snake
{
    public class SnakeGameDrawable : IDrawable
    {
        private readonly MainPage mainPage;
        private const float CELL_PADDING = 1;  // Hücreler arası boşluk

        public SnakeGameDrawable(MainPage mainPage)
        {
            this.mainPage = mainPage ?? throw new ArgumentNullException(nameof(mainPage));
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Arka planı temizle
            canvas.FillColor = Colors.LightGray;
            canvas.FillRectangle(dirtyRect);

            if (mainPage.snakePositions == null || mainPage.snakePositions.Count == 0)
                return;

            // Grid boyutlarını hesapla
            float cellWidth = (float)(dirtyRect.Width / 20);
            float cellHeight = (float)(dirtyRect.Height / 20);

            // Yılanı çiz
            foreach (var position in mainPage.snakePositions)
            {
                canvas.FillColor = Colors.Green;
                canvas.FillRoundedRectangle(
                    (float)(position.X * cellWidth) + CELL_PADDING,
                    (float)(position.Y * cellHeight) + CELL_PADDING,
                    cellWidth - (2 * CELL_PADDING),
                    cellHeight - (2 * CELL_PADDING),
                    3  // Köşe yuvarlaklığı
                );
            }

            // Yılanın başını farklı renkte çiz
            if (mainPage.snakePositions.Count > 0)
            {
                var head = mainPage.snakePositions[0];
                canvas.FillColor = Colors.DarkGreen;
                canvas.FillRoundedRectangle(
                    (float)(head.X * cellWidth) + CELL_PADDING,
                    (float)(head.Y * cellHeight) + CELL_PADDING,
                    cellWidth - (2 * CELL_PADDING),
                    cellHeight - (2 * CELL_PADDING),
                    3
                );
            }

            // Yemeği çiz
            canvas.FillColor = Colors.Red;
            canvas.FillEllipse(
                (float)(mainPage.foodPosition.X * cellWidth) + CELL_PADDING,
                (float)(mainPage.foodPosition.Y * cellHeight) + CELL_PADDING,
                cellWidth - (2 * CELL_PADDING),
                cellHeight - (2 * CELL_PADDING)
            );
        }
    }
} 