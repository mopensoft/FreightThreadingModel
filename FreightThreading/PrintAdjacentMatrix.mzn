array[int, int] of int: rail_links1;
int: nstations;
set of int: Stations = 1..nstations;
predicate isLinked(int: source, int: target) = exists(r in index_set_1of2(rail_links1)) (rail_links1[r,2] == source /\ rail_links1[r,3] == target);
array[Stations, Stations] of var 0..1: adjacents = array2d(Stations, Stations, [ if isLinked(i,j) then 1 else 0 endif | i, j in Stations]);
constraint assert(rail_links1[3,2] == 3 /\ rail_links1[3,3] == 6, "invalid");

solve satisfy;

output 
[ show(adjacents[i,j]) ++ 
  if j == nstations then "\n"
  else ", "
  endif

| i, j in Stations];