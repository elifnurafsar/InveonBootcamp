# InveonBootcamp
Restaurant delivery web app with microservice architecture

## IYZICO Panel Transaction Data
Implemented the integration of the iyzico admin panel to handle customer purchasing data. <br/>
The primary focus is on providing accurate pricing information, considering the application's coupon system and applying discounts effectively.
<br/>
Changes Made:
- Developed a custom pricing technique to apply coupon discounts to products in basket individually.
- Ensured that iyzico receives the correct discounted prices for purchased products.
<br/>

### Example Video Link

https://youtu.be/cqS4Yb1mUsI

<br/>

## Send E-mail Notifications
Implement RabbitMQ message handling and email notifications
This commit introduces a new console web app service within the project, responsible for efficiently processing purchase data messages from RabbitMQ and sending email notifications to users.<br/> 
This functionality ensures that users are promptly informed about their purchases.
<br/>
Changes Made:
- Added a new console web app service for handling purchase data messages.
- Integrated RabbitMQ message processing to consume purchase data.
- Implemented email notifications to notify users about their purchases.

### Example Images
<p align="center">
  <img src="https://github.com/elifnurafsar/InveonBootcamp/assets/60623941/6b276fff-8026-42fe-981e-1b05a2afc272" width="800" alt="Screenshot of Email information that sent by our console app."/>
</p>
<br/>
<p align="center">
  <img src="https://github.com/elifnurafsar/InveonBootcamp/assets/60623941/925bcb03-e012-440c-9aad-f381133a475c" width="800" alt="Screenshot of Iyzico transaction details about our last purchase."/>
</p>
<br/>
<p align="center">
  <img src="https://github.com/elifnurafsar/InveonBootcamp/assets/60623941/a1ca7feb-d724-40bd-8ef8-3b0bd7acb816" width="800" alt="Screenshot of the payment page of our last purchase."/>
</p>





