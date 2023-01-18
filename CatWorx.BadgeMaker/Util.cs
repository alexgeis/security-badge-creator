using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SkiaSharp;

namespace CatWorx.BadgeMaker
{
    class Util
    {
        public static void PrintEmployees(List<Employee> employees)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                // 2nd arg = padding, e.g. -10 = left-aligned 10 space pad
                string template = "{0,-10}\t{1,-20}\t{2}";
                Console.WriteLine(
                    String.Format(
                        template,
                        employees[i].GetId(),
                        employees[i].GetFullName(),
                        employees[i].GetPhotoUrl()
                    )
                );
            }
        }

        public static void MakeCSV(List<Employee> employees)
        {
            // Check to see if folder exists
            if (!Directory.Exists("data"))
            {
                // If not, create it
                Directory.CreateDirectory("data");
            }

            // the keyword "using" has two distinct meanings that depend on the context:
            // 1.  importing a namespace
            // 2. temporarily using a resource)
            // remember that whatever is defined in the parentheses is ONLY available within the subsequent set of curly braces. Once that block of code runs, the resource (in this case, StreamWriter) is removed.
            using (StreamWriter file = new StreamWriter("data/employees.csv"))
            {
                file.WriteLine("ID,Name,PhotoUrl");
                // loop over employees
                for (int i = 0; i < employees.Count; i++)
                {
                    // 2nd arg = padding, e.g. -10 = left-aligned 10 space pad
                    string template = "{0,-10}\t{1,-20}\t{2}";
                    file.WriteLine(
                        String.Format(
                            template,
                            employees[i].GetId(),
                            employees[i].GetFullName(),
                            employees[i].GetPhotoUrl()
                        )
                    );
                }
            }
        }

        async public static Task MakeBadges(List<Employee> employees)
        {
            // LAYOUT VARIABLES
            int BADGE_WIDTH = 669;
            int BADGE_HEIGHT = 1044;

            int PHOTO_LEFT_X = 184;
            int PHOTO_TOP_Y = 215;
            int PHOTO_RIGHT_X = 486;
            int PHOTO_BOTTOM_Y = 517;

            int COMPANY_NAME_Y = 150;

            int EMPLOYEE_NAME_Y = 600;

            int EMPLOYEE_ID_Y = 730;
            // text SKPaint object properties
            SKPaint paint = new SKPaint();
            paint.TextSize = 42.0f;
            paint.IsAntialias = true;
            paint.IsStroke = false;
            paint.TextAlign = SKTextAlign.Center;

            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    SKImage photo = SKImage.FromEncodedData(
                        await client.GetStreamAsync(employees[i].GetPhotoUrl())
                    );
                    SKImage background = SKImage.FromEncodedData(File.OpenRead("badge.png"));

                    SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);
                    SKCanvas canvas = new SKCanvas(badge);

                    // Draw employee image
                    canvas.DrawImage(background, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
                    canvas.DrawImage(
                        photo,
                        new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y)
                    );

                    // Company name
                    paint.Color = SKColors.White;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial");
                    canvas.DrawText(
                        employees[i].GetCompanyName(),
                        BADGE_WIDTH / 2f,
                        COMPANY_NAME_Y,
                        paint
                    );
                    // Employee name
                    paint.Color = SKColors.Black;
                    canvas.DrawText(
                        employees[i].GetFullName(),
                        BADGE_WIDTH / 2f,
                        EMPLOYEE_NAME_Y,
                        paint
                    );
                    // Employee ID
                    paint.Typeface = SKTypeface.FromFamilyName("Courier New");
                    canvas.DrawText(
                        employees[i].GetId().ToString(),
                        BADGE_WIDTH / 2f,
                        EMPLOYEE_ID_Y,
                        paint
                    );

                    SKImage finalImage = SKImage.FromBitmap(badge);
                    SKData data = finalImage.Encode();
                    string template = "data/{0}_badge.png";
                    data.SaveTo(File.OpenWrite(String.Format(template, employees[i].GetId())));
                }
            }
        }

        // public static void MakeBadges(List<Employee> employees)
        // {
        //     // Create image
        //     SKImage newImage = SKImage.FromEncodedData(File.OpenRead("badge.png"));

        //     SKData data = newImage.Encode();

        //     data.SaveTo(File.OpenWrite("data/employeeBadge.png"));
        // }
    }
}
