array[int, int] of int: passenger_train_path;
array[int, int] of int: passenger_train_dtime;
array[int, int] of int: rail_links1;
int: maxStops = 4;
set of int: Trains = index_set_1of2(passenger_train_dtime);
set of int: Links = index_set_2of2(passenger_train_dtime);
array[int, int] of int: passenger_train_etime = array2d(Trains, Links, 
  [passenger_train_dtime[i,j] + rail_links1[passenger_train_path[i,j],4] | i in Trains, j in Links]);

solve satisfy;

output
["<trains>\n"] ++ 
["\t<train id=\"1\">\n"] ++ 
["\t\t<stop sid=\"" ++ show(rail_links1[passenger_train_path[i,j],2]) ++ "\""
     ++ " arrival=\"" ++ int2time(passenger_train_dtime[i,j]) ++ "\""
     ++ " departure=\"" ++ int2time(passenger_train_dtime[i,j]) ++ "\""
     ++ " platform=\"1\"/>\n" 
     ++ if j == maxStops then 
          "\t\t<stop sid=\"" ++ show(rail_links1[passenger_train_path[i,j],3]) ++ "\""
           ++ " arrival=\"" ++ int2time(passenger_train_etime[i,j]) ++ "\""
           ++ " departure=\"" ++ int2time(passenger_train_etime[i,j]) ++ "\""
           ++ " platform=\"1\"/>\n" 
           ++ "\t</train>\n" ++
           if i < card(Trains) then
                "\t<train id=\"" ++ show(i+1) ++ "\">\n"
           else ""      
           endif
        else "" 
        endif 
    | i in Trains, j in Links] ++
["</trains>"];

function string: int2time(int: minutes) = show(minutes div 60) ++ ":" ++ show(minutes mod 60) ++ ":00";