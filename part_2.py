#Initialization//Creation of variable

colors = ['Red', 'Green', 'Blue', 'Yellow', 'Black']
players = [0,1,2,3,4,5,6,7,8,9]

relations = {}
relations[0] = [1,4,5]
relations[1] = [0,2,6]
relations[2] = [1,3,7]
relations[3] = [2,4,8]
relations[4] = [0,3,9]
relations[5] = [0,7,8]
relations[6] = [1,8,9]
relations[7] = [2,5,9]
relations[8] = [3,5,6]
relations[9] = [4,6,7]

colors_of_clusters = {}

Red = []
Blue = []
Green = []
Yellow = []
Black = []

#Greedy Algorithm, Find a color to a player considering its relations
def valideColor(player, color):
  for relation in relations.get(player): 
    color_of_relation = colors_of_clusters.get(relation)
    if color_of_relation == color:
      return False
  return True

#Associate a color for a player using valideColor() 
def get_color_for_player(player):
  for color in colors:
    if valideColor(player, color):
      return color

#Fill the list of clusters with player number 
def create_clusters(player, players, colors_of_clusters):
  if colors_of_clusters[player] == "Red":
    Red.append(player)
  elif colors_of_clusters[player] == "Blue":
    Blue.append(player)
  elif colors_of_clusters[player] == "Green":
    Green.append(player)
  elif colors_of_clusters[player] == "Yellow":
    Yellow.append(player)
  elif colors_of_clusters[player] == "Black":
    Black.append(player)

  return

#Display clusters that have at least a player
def display_clusters():
  if len(Red) != 0:
    print("Red",Red)

  if len(Blue) != 0:
    print("Blue",Blue)

  if len(Green) !=0:
    print("Green",Green)

  if len(Yellow) != 0:
    print("Yellow",Yellow)

  if len(Black) != 0:
    print("Black",Black)

#Only display clusters which not contain player 0
def checkPlayerO():
  valide = True
  for player in Red:
    if player == 0:
      valide = False
  if valide==True:
    print('Red ',Red)

  valide = True
  for player in Blue:
    if player == 0:
      valide = False
  if valide==True:
    print('Blue ',Blue)

  valide = True
  for player in Green:
    if player == 0:
      valide = False
  if valide==True:
    print('Green ',Green)

def main():
  print('\nStep 2: Professor Layton < Guybrush Threepwood < You\n')
  print('\n1| By using graph coloring we find those clusters:\n')
  for player in players:
    colors_of_clusters[player] = get_color_for_player(player)
    create_clusters(player, players, colors_of_clusters)
  display_clusters()
  
  print('\n2| We now eliminate the cluster in which there is player 0:\n')
  checkPlayerO()

  print('\n3| Finally, we eliminate cluster in which at least one player haven\'t seen player 1, 4 or 5\n')
  print('There is our set of Impostors: Green ', Green)

if __name__ == "__main__":
  main()
