set WORKSPACE=..

set GEN_CLIENT=%WORKSPACE%\Data\Luban.ClientServer\Luban.ClientServer.exe
set CONF_ROOT=%WORKSPACE%\Data\Config


%GEN_CLIENT% -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_code_dir %WORKSPACE%/SpacelessTouch/Assets/SpacelessTouch/Tables/Gen ^
 --output_data_dir %WORKSPACE%\SpacelessTouch\Assets\SpacelessTouch\Tables\Bytes ^
 --gen_types code_cs_unity_bin,data_bin ^
 -s all 

pause