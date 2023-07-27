OvetimePolicies - پروژه کتابخانه کلاس و API وب
کتابخانه کلاس (Class Library)

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
        نام و نام خانوادگی
        تاریخ
        حقوق پایه
        حق جذب
        ایاب و ذهاب

OverTimeCalculator:

این پروژه از کتابخانه OvetimePolicies.dll که پیشتر در پروژه OvertimeMethods ایجاد شده است، استفاده می‌کند. این کتابخانه شامل متدهایی برای محاسبه اضافه کار است.
اکشن‌های موجود در API:

    Add:
        این اکشن مبلغ حقوق دریافتی را بر اساس فرمول زیر محاسبه کرده و همراه با اطلاعات ورودی در بانک اطلاعاتی ذخیره می‌کند.

    scss

    نحوه محاسبه حقوق دریافتی = حقوق پایه + حق جذب + ایاب و ذهاب - OverTimeCalculator(مالیات)

    Update:
        این اکشن برای ویرایش اطلاعات یک ماه یک فرد مورد استفاده قرار می‌گیرد.

    Delete:
        این اکشن برای حذف اطلاعات یک ماه یک فرد مورد استفاده قرار می‌گیرد.

    Get:
        این اکشن برای دریافت اطلاعات یک ماه یک فرد مورد استفاده قرار می‌گیرد.

    GetRange:
        این اکشن برای دریافت لیست اطلاعات یک فرد در محدوده زمانی مشخص مورد استفاده قرار می‌گیرد.

نکات مهم:

    برای اکشن‌های اضافه، حذف و ویرایش از EF Core برای اتصال به بانک اطلاعاتی استفاده شود.
    برای اکشن‌های Get و GetRange از Dapper برای اتصال به بانک اطلاعاتی استفاده شود.
    برای مستند سازی API از Swagger استفاده شود.
    UnitTest نیاز است تا کیفیت و عملکرد صحیح پروژه تضمین شود.

نمونه Custom Format:

makefile

فرمت Custom:
نام/نام خانوادگی/تاریخ/حقوق پایه/حق جذب/ایاب و ذهاب
مثال:
John/Doe/2023-07-01/3000/500/200

اطلاعات بالا نشان‌دهنده فردی با نام "John Doe" است که در تاریخ "2023-07-01" حقوق پایه‌ای به مبلغ 3000، حق جذب به مبلغ 500 و ایاب و ذهاب به مبلغ 200 دریافت کرده است.

برای اطلاعات بیشتر درباره استفاده از این پروژه، می‌توانید به مستندات API و کد کتابخانه کلاس مراجعه کنید.
