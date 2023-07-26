using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiHealth.Models;
using prjiHealth.ViewModels;
using prjIHealth.Models;
using prjIHealth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjIHealth.Controllers
{
    public class DiaryController : Controller
    {
        private readonly IHealthContext db;
        public DiaryController(IHealthContext context)
        {
            db = context;
        }
        //IHealthContext db = new IHealthContext();

        //Diary主頁View
        public IActionResult DiaryMain()
        {
            //取得登入者ID
            int userId = 8; //備用帳號
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            CDiaryViewModel diaryViewModel = new CDiaryViewModel(db)
            {
                userId = userId
            };
            string now = DateTime.Now.ToString("yyyyMM" + "32000000");
            double date = double.Parse(now);
            double[] bmis = new double[24];
            for (int i = 0; i < 24; i++)
            {
                CBodyRecordViewModel bodyRecordsViewModel = diaryViewModel.BodyRecords.FirstOrDefault(b => double.Parse(b.FRecordDate) < date);
                if (bodyRecordsViewModel != null)
                    bmis[i] = bodyRecordsViewModel.NumBMI;
                else
                    bmis[i] = 0;
                if (date.ToString().Substring(4, 2) == "01")
                {
                    date = date - 8900000000;
                }
                else
                {
                    date = date - 100000000;
                }
            }
            return View(bmis);
        }
        
        //取得選取日期最接近的身體數據
        public IActionResult getBodyRecord(string date)
        {
            //取得登入者ID
            int userId = 8;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            CDiaryViewModel diaryViewModel = new CDiaryViewModel(db)
            {
                userId = userId
            };

            //沒有資料時顯示最舊一筆
            date = date.Replace("-", "");
            date = date + "235959";
            CBodyRecordViewModel bodyRecordsViewModel = diaryViewModel.BodyRecords.FirstOrDefault(b => double.Parse(b.FRecordDate) < double.Parse(date));
            return Json(bodyRecordsViewModel);
        }

        //新增身體數據
        public IActionResult addBodyRecord(TBodyRecord body)
        {
            //取得登入者ID
            int userId = 8;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            CDiaryViewModel diaryViewModel = new CDiaryViewModel(db)
            {
                userId = userId
            };

            if (body.FHeight != null && body.FWeight != null && body.FWorkload != null)
            {
                //TODO tryparse
                string date = body.FRecordDate.Replace("-", "");
                int count = diaryViewModel.BodyRecords.Where(b => b.FRecordDate.Substring(0, 8) == date).Count();
                double recordDate = double.Parse($"{date}000000") + count;
                TBodyRecord bodyRecord = new TBodyRecord()
                {
                    FMemberId = userId,
                    FRecordDate = recordDate.ToString(),
                    FHeight = body.FHeight,
                    FWeight = body.FWeight,
                    FWorkload = body.FWorkload
                };
                db.TBodyRecords.Add(bodyRecord);
                db.SaveChanges();
            }
            return Content("finish", "text/plain", System.Text.Encoding.UTF8);
        }

        //載入食物列表
        public IActionResult loadAllFoods()
        {
            //取得登入者ID
            int userId = 8;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            CDiaryViewModel diaryViewModel = new CDiaryViewModel(db)
            {
                userId = userId
            };

            //依照使用次數排序
            return Json(diaryViewModel.AllFoods);
        }

        //新增飲食紀錄
        public IActionResult addCalorieIntake(TCalorieIntake intake)
        {
            //取得登入者ID
            int userId = 8;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            //TODO tryparse
            if (intake.FIntakeTime != null && intake.FFoodId != null && intake.FQuantity != null&& intake.FMeal!=null)
            {
                //對同餐別同樣食物不做新增做數量修改
                if (db.TCalorieIntakes.Where(c => c.FMemberId == userId && c.FIntakeTime.Substring(0,8) == intake.FIntakeTime.Replace("-", "") && c.FMeal == intake.FMeal && c.FFoodId == intake.FFoodId).Any())
                {
                    db.TCalorieIntakes.FirstOrDefault(c => c.FMemberId == userId && c.FIntakeTime.Substring(0,8) == intake.FIntakeTime.Replace("-", "") && c.FMeal == intake.FMeal && c.FFoodId == intake.FFoodId).FQuantity += intake.FQuantity;;
                }
                else
                {
                    TCalorieIntake calorieIntake = new TCalorieIntake()
                    {
                        FIntakeTime = intake.FIntakeTime.Replace("-", "") + DateTime.Now.ToString("HHmmss"),
                        FMemberId = userId,
                        FFoodId = intake.FFoodId,
                        FQuantity = intake.FQuantity,
                        FMeal = intake.FMeal
                    };
                    db.TCalorieIntakes.Add(calorieIntake);
                }
                db.SaveChanges();
            }
            
            
            
            
            return Content("finish", "text/plain", System.Text.Encoding.UTF8);
        }     
        
        //搜尋食物
        public IActionResult searchFood(CKeywordViewModel keywordViewModel)
        {
            var foods = new CDiaryViewModel(db).AllFoods.Where(f => f.FFoodName.Contains(keywordViewModel.txtKeyword));
            return Json(foods);
        }   
        
        //驗證食物名是否重複
        public IActionResult checkFoodName(CKeywordViewModel keywordViewModel)
        {
            var foods = new CDiaryViewModel(db).AllFoods.Where(f => f.FFoodName==keywordViewModel.txtKeyword);
            return Json(foods);
        }

        //新增食物
        public IActionResult addFoodCalory(TFoodCalory food)
        {
            if (food.FFoodName != null && food.FUnit != null && food.TCalorieIntakes != null)
            {
                db.TFoodCalories.Add(food);
                db.SaveChanges();
            }
            return Json(food);
        }

        //載入飲食日誌
        public IActionResult getIntakeRecords(string date)
        {

            //取得登入者ID
            int userId = 8;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_Logined_User))
            {
                string json = HttpContext.Session.GetString(CDictionary.SK_Logined_User);
                userId = (JsonSerializer.Deserialize<TMember>(json)).FMemberId;
            }
            CDiaryViewModel diaryViewModel = new CDiaryViewModel(db)
            {
                userId = userId
            };

            date = date.Replace("-", "");
            var intakeRecords = diaryViewModel.CalorieIntakes.Where(c => c.FIntakeTime.Substring(0, 8) == date).Select(c=>new {
                fMeal=c.FMeal,
                fCalorieIntakeId = c.FCalorieIntakeId,
                fFoodName = db.TFoodCalories.FirstOrDefault(f=>f.FFoodId==c.FFoodId).FFoodName,
                fQuantity =c.FQuantity,
                fUnit = db.TFoodCalories.FirstOrDefault(f => f.FFoodId == c.FFoodId).FUnit,
                subtotal = db.TFoodCalories.FirstOrDefault(f => f.FFoodId == c.FFoodId).FCalories*c.FQuantity
            });
            return Json(intakeRecords);
        }

        //修改攝取數量
        public IActionResult editCalorieIntake(TCalorieIntake intake)
        {
            if (intake.FCalorieIntakeId != null && intake.FQuantity != null)
            {
                //TODO tryparse
                db.TCalorieIntakes.FirstOrDefault(c => c.FCalorieIntakeId == intake.FCalorieIntakeId).FQuantity = intake.FQuantity;
                db.SaveChanges();
            }
            return Content("finish", "text/plain", System.Text.Encoding.UTF8);
        }

        //刪除飲食紀錄
        public IActionResult delCalorieIntake(int FCalorieIntakeId)
        {
            if (FCalorieIntakeId != null)
            {
                //TODO tryparse
                var calorieIntakes = db.TCalorieIntakes.FirstOrDefault(c => c.FCalorieIntakeId == FCalorieIntakeId);
                db.TCalorieIntakes.Remove(calorieIntakes);
                db.SaveChanges();
            }
            return Content("finish", "text/plain", System.Text.Encoding.UTF8);
        }
    }
}
