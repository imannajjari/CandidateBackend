OvetimePolicies - پروژه کتابخانه کلاس و API وب
کتابخانه کلاس (Class Library)

برای اجرای پروژه بر روی این لینک کلیک کنید: https://candidatebackend.iran.liara.run/

در این پروژه، یک کتابخانه کلاس به نام OvetimePolicies ایجاد شده است که شامل سه متد برای محاسبه اضافه کار می‌باشد.
نام متدها:

    CalcurlatorC
    CalcurlatorB
    CalcurlatorA

API وب (Web API)

این پروژه شامل یک وب API است که اطلاعات ماهانه افراد را به صورت ورودی دریافت کرده، مبلغ حقوق دریافتی کارکنان را محاسبه کرده و اطلاعات دریافتی را در بانک اطلاعاتی ذخیره می‌کند.
ورودی API:

    فرمت اطلاعات ورودی این API متفاوت است که می‌تواند به صورت Json، Xml، Cs یا Custom باشد. آدرس API به شرح زیر معین می‌شود:

bash

{datatype}/controller/action/
mysite.com

    فرمت Custom به این شکل است که در سطر اول ترتیب عناوین تعریف شده و با علامت '/' تفکیک شده است و در سطر دوم مقادیر آن‌ها آمده‌اند. یک نمونه مثال از این فرمت در انتهای این سند نمایش داده شده است.

    اطلاعات ورودی شامل موارد زیر می‌شود:
        نام 
        نام خانوادگی
        حقوق پایه
        حق جذب
        ایاب و ذهاب
        تاریخ
        ساعت اضافه کاری

OverTimeCalculator:

این پروژه از کتابخانه OvetimePolicies.dll که پیشتر در پروژه OvertimeMethods ایجاد شده است، استفاده می‌کند. این کتابخانه شامل متدهایی برای محاسبه اضافه کار است.
اکشن‌های موجود در API:

    Add:
        این اکشن مبلغ حقوق دریافتی را بر اساس فرمول زیر محاسبه کرده و همراه با اطلاعات ورودی در بانک اطلاعاتی ذخیره می‌کند.

    Calculare:

    نحوه محاسبه حقوق دریافتی = حقوق پایه + حق جذب + ایاب و ذهاب + OverTimeCalculator(حقوق پایه ، حق جذب ، ساعت اضافه کاری)- مالیات

    Update:
        این اکشن برای ویرایش اطلاعات یک ماه یک فرد مورد استفاده قرار می‌گیرد.

    Delete:
        این اکشن برای حذف اطلاعات یک ماه یک فرد مورد استفاده قرار می‌گیرد.

    Get:
        این اکشن برای دریافت اطلاعات یک ماه یک فرد مورد استفاده قرار می‌گیرد.

    GetRange:
        این اکشن برای دریافت لیست اطلاعات یک فرد در محدوده زمانی مشخص مورد استفاده قرار می‌گیرد.

نکات مهم:

    برای اکشن‌های اضافه، حذف و ویرایش از EF Core برای اتصال به بانک اطلاعاتی استفاده می شود.
    برای اکشن‌های Get و GetRange از Dapper برای اتصال به بانک اطلاعاتی استفاده می شود.
    برای دسترسی به داکیومنت API ها از آدرس https://candidatebackend.iran.liara.run/Swagger استفاده شود.
    این پروژه دارای یونیت تست جهت تست سه متد CalculateA , CalculateB , CalculateC می باشد

نمونه Custom Format:



فرمت Custom:
نام/نام خانوادگی/کد پرسنلی/حقوق پایه/حق جذب/ایاب و ذهاب/تاریخ/ساعت اضافه کاری
مثال:
Iman/Najjari/3333/14000/120/160/14020509/47

اطلاعات بالا نشان‌دهنده فردی با نام "Iman Najjari" و پرسنلی 3333 است که در تاریخ "14020509" حقوق پایه‌ای به مبلغ 14000، حق جذب به مبلغ 120 و ایاب و ذهاب به مبلغ 160 دریافت کرده و 47 ساعت اضافه کاری دارد.

برای اطلاعات بیشتر درباره استفاده از این پروژه، می‌توانید به مستندات API و کد کتابخانه کلاس مراجعه کنید.

نکات اجرای پروژ:

    1- برای تست API ها با Postman فایل Candidate.postman_collection در پوشه ProjectResources قرار دارد.
    2- برای ایجاد دیتابیس فایل Create DB در پوشه ProjectResources قرار دارد.
    3- برای ایجاد دیتای تستی اولیه فایل Initial TestData در پوشه ProjectResources قرار دارد.
