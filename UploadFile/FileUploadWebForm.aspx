<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUploadWebForm.aspx.cs" Inherits="UploadFile.FileUploadWebForm" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload File</title>
    <script src="https://www.google.com/recaptcha/api.js?onload=renderRecaptcha&render=explicit" async defer></script>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js" ></script>
    <script type="text/javascript">     
        var captchaValid;
        var hasFiles;

        $(document).ready(function () {
            $("#btnUploadFile").attr('disabled', 'disabled');
            captchaValid = false;

           document.getElementById('fileUpload').onchange = function (e) {
                var fileInput = document.getElementById('fileUpload');
                if (fileInput.files.length > 0) {
                    hasFiles = true;
                    if (captchaValid) {
                        $("#btnUploadFile").removeAttr('disabled');
                    }
                }
                else {
                    hasFiles = false;
                }
            };

            $('#btnUploadFile').on('click', function () {
                var data = new FormData();
                var fileInput = document.getElementById('fileUpload');
                if (fileInput.files.length > 0) {
                    
                    if ((/\.(bmp|jpg|jpeg|pdf|png)$/i).test(fileInput.files[0].name)) {
                        data.append(fileInput.files[0].name, fileInput.files[0]);
                    }
                    else {
                        alert('You must select a valid file (bmp,jpg,jpeg,pdf,png)');          
                        return false;
                    }
                }

                var ajaxRequest = $.ajax({
                    type: "POST",
                    url: "/api/fileupload/uploadfile",
                    contentType: false,
                    processData: false,
                    data: data
                });

                ajaxRequest.done(function (response, textStatus) {
                    if (textStatus == 'success') {
                        if (response != null) {
                            if (response.Key) {
                                alert(response.Value);
                                $("#fileUpload").val('');
                                grecaptcha.reset();
                            } else {
                                alert(response.Value);
                            }
                        }
                    } else {
                        alert(response.Value);
                    }
                });
            });
        });

        var your_site_key = '<%= ConfigurationManager.AppSettings["SiteKey"]%>';  
        var renderRecaptcha = function () {  
            grecaptcha.render('ReCaptchContainer', {  
                'sitekey': your_site_key,  
                'callback': reCaptchaCallback,  
                theme: 'light',
                type: 'image',  
                size: 'normal'    
            });  
        };  
  
    var reCaptchaCallback = function (response) {  
        if (response !== '') {
            captchaValid = true;
            if (hasFiles) {
                $("#btnUploadFile").removeAttr('disabled');
            }
            
        }
        else {
            captchaValid = false;
            document.getElementById("btnUploadFile").enable = false;
        }
    };  
  
    jQuery('button[type="button"]').click(function(e) {  
        var message = 'Please check the checkbox';  
        if (typeof (grecaptcha) != 'undefined') {  
            var response = grecaptcha.getResponse();  
            (response.length === 0) ? (message = 'Captcha verification failed') : (message = 'Success!');  
        }  
        jQuery('#lblMessage').html(message);  
        jQuery('#lblMessage').css('color', (message.toLowerCase() == 'success!') ? "green" : "red");  
        });  

         var onloadCallback = function() {
        grecaptcha.render('html_element', {
          'sitekey' : 'your_site_key'
        });
      };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="fileUpload">Select File to Upload:</label>
            <input type="file" id="fileUpload" accept="image/jpg,image/png,image/jpeg,image/bmp,application/pdf" />
            <br />
               <label id="lblMessage" runat="server" clientidmode="static"></label>  
        </div>
        <br />
            <br />
        <div id="ReCaptchContainer" class="g-recaptcha" data-size="normal" >
         
        </div>
        <div>
        <input type="button" value="Upload File" id="btnUploadFile" />
           
       </div>
    </form>
    
</body>
</html>
