% ===================================================================================  %
% Minizinc model used in the paper "Freight Train Threading with Different Algorithms" %
% Authors: Ilankaikone Senthooran, Mark Wallace  and Leslie De                         %
% Last modified: December 22nd, 2014                                                   %
% ===================================================================================  %
include "ft16.dzn";
include "test1.dzn";
%include "loop_test1.dzn";

int: O = 1; int: D = 2; %O - Origin, D - Destination
int: LINKDURATION = 4; %Fourth column of rail_links array gives the time taken by a passenger train to travel the corresponding link.
int: crosslinks = 5;%5 for test*.dzn, 10 for loop_test*.dzn. %max([i| i in index_set_1of2(coupled_links_four)]); % Total number of cross links
int: coupledlinks = 5; % this should be readable from input array 
int: plinks = 4 ; % Passenger train path length - IS THIS A MAX LENGTH OR PAX TRAIN HAS FIXED LENGTH

int: npassenger_trains; %Total no. of passenger train that runs in a day in the given railway network.
int: nfreight_trains; %No of freight trains to be scheduled in a day.
int: maxstops; % Maximum no. of stops that a freight train is allowed to take, including both originating and destination station
int: nlinks; %Total no. of railway links available in the given rail network
int: nstations; %Total no. of railway station available in the given rail network - % nstations - not used??? WHY NOT?
int: time_gap; %Minimum time gap between trains - HEADWAY CONDITION
%  Speed factor assumed to be 1 in this model. PAX TRAINS HAVE FIXED SPEED. EACH FREIGHT TRAIN HAS DIFFERENT SPEED COMPARED TO PAX TRAINS
array[FreightTrains] of int: speed_factor; %Ratio between passenger train speed and freight train speed.
int: speed_mul; %multiplied with speed_factor array to get different speed ratio for different scenarios.

set of int: FreightTrains = 1..nfreight_trains;
set of int: PassengerTrains = 1..npassenger_trains;
set of int: RailLinks = 1..nlinks;
set of int: Stations = 1..nstations;
set of int: MaxStops = 1..maxstops;
set of int: CrossLinks = 1..crosslinks;
set of int: CoupledLinks = 1..coupledlinks;

%Contains all rail links, their end stands and the travel duration in that link.
array[RailLinks,1..4] of int: rail_links1; % FROM INPUT DATA
array[0..nlinks,1..4] of int: rail_links = array2d(0..nlinks,1..4,[if i=0 then 0 else rail_links1[i,j] endif | i in 0..nlinks, j in 1..4]); % WITH LINK 0
constraint assert((forall (i in RailLinks)(rail_links[i,1] in RailLinks)), "Invalid data file" ++ "Error in number of rail links");
constraint assert((forall (i in RailLinks, j in 2..3)(rail_links[i,j] in Stations)), "Invalid data file" ++ "Rail link connects non-existent stations");
constraint assert((forall (i in RailLinks)(rail_links[i,4] >= 0)), "Invalid data file" ++ "Rail link duration cannot be negative");

%Contains all passenger trains and their travel link in sequential order.
array[PassengerTrains,1..plinks] of int: passenger_train_path; % INPUT DATA
constraint assert((forall (i in PassengerTrains, j in 1..plinks)(passenger_train_path[i,j] in RailLinks)), "Invalid data file" ++ "Passenger train path includes non-existent links"); % check if link number are valid
constraint assert((forall (i in PassengerTrains, j in 1..plinks-1)(rail_links[passenger_train_path[i,j], 3] == rail_links[passenger_train_path[i,j+1],2])), "Invalid link connection of passenger train"); % MV-Check if links of a train are connected

%Contains all passenger trains departure time at relevant link.
array[PassengerTrains,1..plinks] of int: passenger_train_dtime; % INPUT DATA
constraint assert((forall (i in PassengerTrains, j in 1..plinks)(passenger_train_dtime[i,j] >= 0)), "Invalid data file" ++ "Passenger train departure times includes negative time");
constraint assert((forall (i in PassengerTrains, j in 1..plinks-1)(passenger_train_dtime[i,j] + rail_links[passenger_train_path[i,j],4] = passenger_train_dtime[i,j+1])), "Invalid passenger train running time because stop time is not in incremental order"); % MV-Check if running time on each link of the train are correct

%Origin and destation stations of the Freight Trains need to be scheduled. 
array[FreightTrains,O..D] of int: freight_trains; % INPUT DATA
constraint assert((forall (i in FreightTrains, j in O..D)(freight_trains[i,j] in Stations)), "Invalid data file" ++ "Freight train connects non-existent stations");
array[CoupledLinks,1..2] of int: coupled_links; % Rail links that have same originating stations - WHY ONLY 2 LINKS, WHAT IF 3+ LINKS HAS SAME ORIGIN
array[CrossLinks,1..4] of int: coupled_links_four; % Rail links that have same originating stations

% Passenger train start time at each link and 0 if link r is not path of passenger path
array[PassengerTrains,RailLinks] of int: PTT = 
    array2d(PassengerTrains,RailLinks,
      [sum(j in 1..plinks)(bool2int(r = passenger_train_path[i,j])*passenger_train_dtime[i,j]) | i in PassengerTrains, r in RailLinks] );

array[FreightTrains,MaxStops] of var 0..nlinks: freight_train_path; %.[.,L] - Lth link. frieght train path in sequential order.
array[FreightTrains,MaxStops] of var 0..1440: freight_train_dtime; %.[.,L] - Departure time at Lth link.
array[FreightTrains,FreightTrains] of var bool: order;
array[FreightTrains] of var MaxStops: stopcount;
array[FreightTrains,RailLinks] of var bool: freight_train_sx; % [i,j] specifies whether train i take link j or not

%%Constraint 8: Daily operational times of freight trains - ???: does it show how train stop at stations?
array[FreightTrains,RailLinks] of var 0..1440: freight_train_lb; % ??? Lower bound start time
array[FreightTrains,RailLinks] of var -100..1440: freight_train_ub; % ??? Upper bound start time
array[FreightTrains,RailLinks] of var 0..1440: end_lb; % ??? Lower bound end time
array[FreightTrains,RailLinks] of var -100..1440: end_ub; % ??? Upper bound end time

%%Constraint 2: Start location of freight trains
constraint forall(i in FreightTrains)
             (rail_links[freight_train_path[i,1],2] = freight_trains[i,1]) ;

%%Constraint 3 & 1: End location of freight trains & Length of Path - INTERESTINGLY THIS CONSTRAINT DEFINES STOPCOUNT[i] VALUE
constraint forall(i in FreightTrains)
             (rail_links[freight_train_path[i,stopcount[i]],3] = freight_trains[i,2]) ;

%%Constraint 4: Preceding railway links: links of freight train are connected
% constraint forall(i in FreightTrains, j in 1..maxstops-1)
%              (stopcount[i]>j -> 
%                  rail_links[freight_train_path[i,j],3] = rail_links[freight_train_path[i,j+1],2]) ;

constraint forall(i in FreightTrains, j in 1..stopcount[i]-1)
             (rail_links[freight_train_path[i,j],3] = rail_links[freight_train_path[i,j+1],2]) ;

%%Constraint 1b: Set the remaining freight train links are set to 0 (after stopcount)
constraint forall(i in FreightTrains,j in MaxStops)
            ( j > stopcount[i] -> freight_train_path[i,j] = 0);

%%Constraint 6: Freight trains that use the same railway link have a precedence between them
%Set true for the paths that are been used
constraint
    forall(i in FreightTrains, j in MaxStops)
            (j <= stopcount[i] -> freight_train_sx[i,freight_train_path[i,j]]);
%Set false for the paths that not been used
constraint
  forall(r in RailLinks, i in FreightTrains)
        ( forall(j in MaxStops) (freight_train_path[i,j] != r) -> freight_train_sx[i,r] = false);

%Trains share the same link need to be in order
constraint
    forall(i1,i2 in FreightTrains, r in RailLinks where i2!=i1)
          ((freight_train_sx[i1,r]) /\ (freight_train_sx[i2,r])
              ->        (order[i1,i2] \/ order[i2,i1]) );
%% If the freight train uses the link, then the upper and lower bounds 
% of the optional variables are set to be the same 
% to reflect that the variables are now actual
constraint
    forall(i in FreightTrains, r in RailLinks)
           (freight_train_sx[i,r] ->
             freight_train_lb[i,r] = freight_train_ub[i,r] /\
             end_lb[i,r] = end_ub[i,r]) ;

% Pair of trains are not sharing the same link or crossing each other, do not need to be in order - WHY IS THIS NECESSARY
constraint
    forall(i1,i2 in FreightTrains where i1!=i2) (
        sum(r in RailLinks) (bool2int(freight_train_sx[i1,r] /\ freight_train_sx[i2,r] )) = 0  % i1, i2 don't share any links
        /\
       ( 
            sum(j in CrossLinks)( % i1, i2 are not crossing each other. NOTE: ONLY 1-4 CROSS 2-3 IN CROSS LINK, 1-3 AND 2-3 ARE NOT CROSSING
                bool2int(
                   (freight_train_sx[i1,coupled_links_four[j,1]] /\ freight_train_sx[i1,coupled_links_four[j,4]] /\
                   freight_train_sx[i2,coupled_links_four[j,2]] /\ freight_train_sx[i2,coupled_links_four[j,3]])
                \/
                   %THIS SEEMS REDUNDANCE if i1 != i2. ONLY NEEDED if i1 < i2 but when is removed, it cause ISSUES
                   (freight_train_sx[i2,coupled_links_four[j,1]] /\ freight_train_sx[i2,coupled_links_four[j,4]] /\
                   freight_train_sx[i1,coupled_links_four[j,2]] /\ freight_train_sx[i1,coupled_links_four[j,3]])
                )
            ) = 0
        )
        -> (order[i1,i2] = false /\ order[i2,i1] = false ) ); 

%For a pair of trains both precedence boolean cannot be true at the same time - SIDE constraint 
constraint forall(i1,i2 in FreightTrains where i2>i1) 
             (not (order[i1,i2] /\ order[i2,i1]) ) ;

%Precedence constraint on trains those have same start and end stations
%Reduces search space. This constraint was not used to produce the figures in the paper. 
% By making assumption that i1 always start before i2, this also set the order of trains to break symmetric
% THIS COULD BE CHANGE OR REMOVE TO ALLOW OVER TAKING AS LONG AS IT KEEP THE HEADWAY CONTRAINST
constraint
  forall(i1,i2 in FreightTrains where i1 < i2)
    (((freight_trains[i1,1] = freight_trains[i2,1]) /\ (freight_trains[i1,2] = freight_trains[i2,2]))
      ->
      order[i1,i2] = true);

%%Side Constraint 1: lines crossing constraint - if two train paths cross, then they must be separated by a headway
constraint forall(i1,i2 in FreightTrains, p in CrossLinks where i1!=i2)
                  (( (freight_train_sx[i1,coupled_links_four[p,1]]  /\ freight_train_sx[i1,coupled_links_four[p,4]])
                            /\ (freight_train_sx[i2,coupled_links_four[p,2]]  /\ freight_train_sx[i2,coupled_links_four[p,3]])
                  \/
                   %THIS IS REDUNDANCE if i1 != i2. ONLY NEEDED if i1 < i2
                   (freight_train_sx[i2,coupled_links_four[p,1]] /\ freight_train_sx[i2,coupled_links_four[p,4]] /\
                   freight_train_sx[i1,coupled_links_four[p,2]] /\ freight_train_sx[i1,coupled_links_four[p,3]])
                    )
                       -> (order[i1,i2] \/ order[i2,i1])); %%% NOTE: this is not consistent with the previous one.

%%Constraint 7: Minimum headway time between trains is Gap
%freight-freight
constraint
    forall(i1,i2 in FreightTrains, r in RailLinks where i1!=i2)
          ((
          freight_train_sx[i1,r] ) /\ (freight_train_sx[i2,r] /\ order[i2,i1])
              ->
            (freight_train_lb[i1,r] >= freight_train_ub[i2,r] + time_gap /\ % start time on link r
               end_lb[i1,r] >= end_ub[i2,r] + time_gap) ); % end time on link r

constraint forall(f in FreightTrains, r in RailLinks)
    ( end_lb[f,r] >= freight_train_ub[f,r]+rail_links[r,LINKDURATION]*speed_factor[f]*speed_mul); % running time constraint for Freight trains

constraint forall(f in FreightTrains, r1, r2 in RailLinks where rail_links[r1,3]=rail_links[r2,2])
    ( end_ub[f,r1] <= freight_train_lb[f,r2]); % link arrival and next link departure constraints

bool: canstop = false; 

constraint % if canstop then true else
    forall(f in FreightTrains, r1, r2 in RailLinks where rail_links[r1,3]=rail_links[r2,2])
    (if canstop then end_ub[f,r1] <= freight_train_lb[f,r2]
                else end_ub[f,r1] = freight_train_lb[f,r2] endif);

%==========Cumulative constraint - start============================

%--------------Opturion-cpx - start--------------------
include "globals.mzn";

constraint forall(r in RailLinks)(cumulative_optional(
        [ freight_train_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r] | p in PassengerTrains where PTT[p, r] > 0 ], % start time list
        [ freight_train_ub[f, r] | f in FreightTrains ] ++ [ PTT[p, r] | p in PassengerTrains where PTT[p, r] > 0 ], % duration list 
        [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ true | p in PassengerTrains where PTT[p, r] > 0 ], % resource list
        [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains where PTT[p, r] > 0 ], % boolean list
        [ 1 | f in FreightTrains ] ++ [ 1 | p in PassengerTrains where PTT[p, r] > 0 ],
        1
    ));% :: ttef);

constraint forall(f in FreightTrains, r in RailLinks)
        (end_lb[f,r] >= freight_train_ub[f,r]+rail_links[r,LINKDURATION]*speed_factor[f]*speed_mul);
constraint forall(f in FreightTrains, r1, r2 in RailLinks where rail_links[r1,3]=rail_links[r2,2])
    ( end_ub[f,r1] <= freight_train_lb[f,r2]);
constraint forall(r in RailLinks)(cumulative_optional(
        [ end_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r]+rail_links[r,LINKDURATION] | p in PassengerTrains where PTT[p, r] > 0 ],
        [ end_ub[f, r] | f in FreightTrains ] ++ [ PTT[p, r]+rail_links[r,LINKDURATION] | p in PassengerTrains where PTT[p, r] > 0 ],
        [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ true | p in PassengerTrains where PTT[p, r] > 0 ],
        [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains where PTT[p, r] > 0 ],
        [ 1 | f in FreightTrains ] ++ [ 1 | p in PassengerTrains where PTT[p, r] > 0 ],
        1
    ));% :: ttef);


% %==========Cumulative constraint - start============================
% int: hzn = 1440; % Horizon
% constraint forall(r in RailLinks, f in FreightTrains, p in PassengerTrains where PTT[p,r] > 0)
%         (freight_train_lb[f, r] in 0..hzn diff PTT[p,r]+1-time_gap .. PTT[p,r]+time_gap-1
%          /\
%          freight_train_ub[f, r] in 0..hzn diff PTT[p,r]+1-time_gap .. PTT[p,r]+time_gap-1) ;


% constraint forall(r in RailLinks, f in FreightTrains, p in PassengerTrains where PTT[p,r] > 0)
%         (end_lb[f, r] in 0..hzn diff PTT[p,r]+rail_links[r,LINKDURATION]+1-time_gap .. 
%                             PTT[p,r]+rail_links[r,LINKDURATION]+time_gap-1
%          /\
%          end_ub[f, r] in 0..hzn diff PTT[p,r]+rail_links[r,LINKDURATION]+1-time_gap .. 
%                             PTT[p,r]+rail_links[r,LINKDURATION]+time_gap-1);

%--------------Gecode - start--------------------

% include "gecode.mzn";

% constraint forall(r in RailLinks)(gecode_schedule_cumulative_optional(
%         [ freight_train_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r] | p in PassengerTrains ],
%         [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains ],
%         [ 1 | f in FreightTrains ] ++ [ if PTT[p, r] > 0 then 1 else 0 endif | p in PassengerTrains ],
%         [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ PTT[p, r] > 0 | p in PassengerTrains ],
%         1
%     ));

% constraint forall(r in RailLinks)(gecode_schedule_cumulative_optional(
%         [ end_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r]+rail_links[r,LINKDURATION] | p in PassengerTrains ],
%         [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains ],
%         [ 1 | f in FreightTrains ] ++ [ if PTT[p, r] > 0 then 1 else 0 endif | p in PassengerTrains ],
%         [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ PTT[p, r] > 0 | p in PassengerTrains ],   
%         1
%     ));

% constraint forall (f in FreightTrains, r in RailLinks) (freight_train_sx[f,r] -> freight_train_ub[f, r] = freight_train_lb[f, r]) ;
% constraint forall (f in FreightTrains, r in RailLinks) (freight_train_sx[f,r] -> end_ub[f, r] = end_lb[f, r]) ;

%--------------Gecode - end--------------------

%--------------Search for Gecode: start---------------
/*

          seq_search([
                    int_search([freight_train_path[i,r]|r in MaxStops, i in FreightTrains], first_fail, indomain_min,complete),
                    bool_search([order[i,j]| i,j in FreightTrains], first_fail, indomain_min,complete),                    
                    bool_search([freight_train_sx[i,r]| r in RailLinks, i in FreightTrains], first_fail, indomain_min,complete),
                    int_search([freight_train_lb[i,r]|r in RailLinks, i in FreightTrains], smallest, indomain_min,complete),
                    int_search([end_lb[i,r]|r in RailLinks, i in FreightTrains], smallest, indomain_min,complete)])
*/
%--------------Search for Gecode: End---------


%%Objective function - Minimise the time of the last train in the network.
var 0..1440: Latest;
constraint forall(i in FreightTrains)(Latest >= freight_train_lb[i,freight_train_path[i,stopcount[i]]]) ;

/*
predicate cumulative_optional(
        array[int] of var int: sl,
        array[int] of var int: su,
        array[int] of var bool: sx,
        array[int] of var int: d,
        array[int] of var int: r, var int: b) =
        forall(i in index_set(sl))
            (sl[i] >= su[i] /\ (sx[i] -> sl[i] = su[i]) /\ (not(sx[i]) -> su[i] < sl[i])) /\
        cumulative_optional_fd_g12_cpx(sl, su, sx, fix(d), fix(r), b);

%-----------------------------------------------------------------------------%

predicate cumulative_optional_fd_g12_cpx(
        array[int] of var int: sl,
        array[int] of var int: su,
        array[int] of var bool: sx,
        array[int] of int: d,
        array[int] of int: r, var int: b);

%-----------------------------------------------------------------------------%

annotation tt;
annotation ttef;
*/
solve ::

%Alternative search routine (improved). This routine was not used to produce the figures in the paper.
%limited_search(time_limit(600.0),
%cpx_warm_start(5000,
restart_luby(time_limit(0.5),
          seq_search([
                      int_search([freight_train_path[i,r]|r in MaxStops, i in FreightTrains], cpx_max_activity, indomain_min,complete),
%                    bool_search([order[i,j]| i,j in FreightTrains], cpx_max_activity, indomain_min,complete),                    
                    bool_search([freight_train_sx[i,r]| r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
                    int_search([freight_train_lb[i,r]|r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
                    int_search([end_lb[i,r]|r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
                    default_search
]) %End seq_search
) %End restart_luby

minimize Latest; 

% %Prints information on to the console
% output 
% ["StopCount = \n"]++[ show_int(3,stopcount[i]) ++ " " ++
%     if i == 0 then "\n" else "" endif
%         | i in FreightTrains] ++ ["\n"] ++

% ["Freight Train Path (links) = \n"]++[ show_int(3,freight_train_path[i,j]) ++ " " ++
%    if j == maxstops then "\n" else "" endif
%         | i in FreightTrains,j in MaxStops ] ++ ["\n"] ++

% ["Freight Train Time (links) = \n"]++[ show_int(3,freight_train_lb[i,freight_train_path[i,j]]) ++ " " ++
%    if j == maxstops then "\n" else "" endif
%         | i in FreightTrains,j in MaxStops ] ++ ["\n"] ++

% ["Time Table = \n"]++[ show(if not fix(freight_train_sx[i,j]) then 0 else freight_train_lb[i,j] endif) ++ "\t" ++
%     if j == nlinks then "\n" else "" endif
%          | i in FreightTrains, j in RailLinks] ++ ["\n"] ++

% ["Last train arrival time is "] ++ [ show(Latest) ] ; 

%% ---------- XML Output start -------------------%%

set of int: Links = index_set_2of2(passenger_train_dtime);
% Passenger train arrival time
array[int, int] of int: passenger_train_etime = array2d(PassengerTrains, Links, 
  [passenger_train_dtime[i,j] + rail_links1[passenger_train_path[i,j],4] | i in PassengerTrains, j in Links]);
% % Freight train arrivale time
% array[int, int] of var int: freight_train_etime = array2d(FreightTrains, MaxStops, 
%   [freight_train_dtime[i,j] + rail_links1[freight_train_path[i,j],4] | i in FreightTrains, j in MaxStops]);
function string: int2time(int: minutes) = show(minutes div 60) ++ ":" ++ show(minutes mod 60) ++ ":00";
function string: int2time(var int: minutes) = show(minutes div 60) ++ ":" ++ show(minutes mod 60) ++ ":00";

output
["Last train arrival time is "] ++ [ show(Latest) ] ++ ["\n"] ++ 
["<timetable>\n"] ++
% Print stations
["<stations>\n"] ++
["\t<station id = \"" ++ show(i) ++ "\" name = \"S" ++ show(i) ++ "\"/>\n" | i in Stations] ++
["</stations>\n"] ++
% Print links
["<links>\n"] ++
["\t<link id=\"" ++ show(rail_links[i,1]) ++ "\" source=\"" ++ show(rail_links[i,2]) ++ "\" target=\"" ++ show(rail_links[i,3]) ++ "\" traveltime=\"" ++ show(rail_links[i,4]) ++ "\" />\n" | i in RailLinks] ++
["</links>\n"] ++
["<trains>\n"] ++ 
% Print Passenger trains
["\t<train id=\"P1\">\n"] ++ 
["\t\t<stop sid=\"" ++ show(rail_links1[passenger_train_path[i,j],2]) ++ "\""
     ++ " arrival=\"" ++ int2time(passenger_train_dtime[i,j]) ++ "\""
     ++ " departure=\"" ++ int2time(passenger_train_dtime[i,j]) ++ "\""
     ++ " platform=\"1\"/>\n" 
     ++ if j == maxstops then 
          "\t\t<stop sid=\"" ++ show(rail_links1[passenger_train_path[i,j],3]) ++ "\""
           ++ " arrival=\"" ++ int2time(passenger_train_etime[i,j]) ++ "\""
           ++ " departure=\"" ++ int2time(passenger_train_etime[i,j]) ++ "\""
           ++ " platform=\"1\"/>\n" 
           ++ "\t</train>\n" ++
           if i < card(PassengerTrains) then
                "\t<train id=\"P" ++ show(i+1) ++ "\">\n"
           else ""      
           endif
        else "" 
        endif 
    | i in PassengerTrains, j in Links] ++
%Print Freight trains
["\t<train id=\"F1\">\n"] ++ 
["\t\t<stop sid=\"" ++ show(rail_links1[freight_train_path[i,j],2]) ++ "\""
     ++ " arrival=\"" ++ int2time(freight_train_dtime[i,j]) ++ "\""
     ++ " departure=\"" ++ int2time(freight_train_dtime[i,j]) ++ "\""
     ++ " platform=\"1\"/>\n" 
     ++ if j == fix(stopcount[i]) then 
          "\t\t<stop sid=\"" ++ show(rail_links1[freight_train_path[i,j],3]) ++ "\""
           ++ " arrival=\"" ++ int2time(freight_train_dtime[i,j] + rail_links1[freight_train_path[i,j],4]) ++ "\""
           ++ " departure=\"" ++ int2time(freight_train_dtime[i,j] + rail_links1[freight_train_path[i,j],4]) ++ "\""
           ++ " platform=\"1\"/>\n" 
           ++ "\t</train>\n" ++
           if i < card(FreightTrains) then
                "\t<train id=\"F" ++ show(i+1) ++ "\">\n"
           else ""      
           endif
        else "" 
        endif 
    | i in FreightTrains, j in 1..fix(stopcount[i])] ++

["</trains>\n"] ++
["</timetable>"];

%% ------------------- XML Output end -------------------------
