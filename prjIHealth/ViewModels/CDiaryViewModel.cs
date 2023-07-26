using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiHealth.Models;
using prjiHealth.ViewModels;
using prjIHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjIHealth.ViewModels
{
    public class CDiaryViewModel
    {
        private readonly IHealthContext db;
        public CDiaryViewModel(IHealthContext context)
        {
            db = context;
        }
        //IHealthContext db = new IHealthContext();
        public int userId = 8;


        //載入全部食物列表
        public IEnumerable<TFoodCalory> AllFoods
        {
            get
            {
                return db.TFoodCalories.OrderByDescending(f => f.TCalorieIntakes.Where(c => c.FMemberId == userId).Count()).ThenBy(f => f.FFoodId);
            }
        }


        //登入會員的身體數據
        public IEnumerable<CBodyRecordViewModel> BodyRecords
        {
            get
            {
                var q =db.TBodyRecords.Where(b => b.FMemberId == userId).OrderByDescending(b => b.FRecordDate);
                List<CBodyRecordViewModel> bodyRecords = new List<CBodyRecordViewModel>();
                foreach(var b in q)
                {
                    CBodyRecordViewModel bodyRecord = new CBodyRecordViewModel(db)
                    {
                        FBodyRecordId = b.FBodyRecordId,
                        FMemberId = b.FMemberId,
                        FRecordDate = b.FRecordDate,
                        FWorkload = b.FWorkload,
                        FHeight = b.FHeight,
                        FWeight = b.FWeight
                    };
                    bodyRecords.Add(bodyRecord);
                }
                return bodyRecords;
            }
        }

        //登入會員的飲食日誌
        public IEnumerable<TCalorieIntake> CalorieIntakes
        {
            get
            {
                var intakeRecords = db.TCalorieIntakes.Where(c=>c.FMemberId == userId).OrderBy(c => c.FIntakeTime);
                return intakeRecords;
            }
        }

    }
}
