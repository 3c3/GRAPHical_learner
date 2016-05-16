#include <iostream>
#include <algorithm>
#include <ctime>
#include <stack>

#include "GraphicalLearnerNative.h"

using namespace std;

#define MAXN 10000
#define MAXM 100000

class DisjointSetForest
{
private:
	int rank[MAXN]; //Относително равно на височината на дърво, зависи колко пъти е викана find
public:
	int parent[MAXN];
	int setCount;

	void create(int cNodes)
	{
		setCount = cNodes;
		for (int i = 0; i < cNodes; i++) //Върховете почват от 0
		{
			parent[i] = i;
			rank[i] = 0;
		}
	}
	int find(int node)
	{
		if (parent[node] != node) parent[node] = find(parent[node]); //Всички звена по пътя вързваме директно към корена
		return parent[node];
	}
	void merge(int root1, int root2)
	{
		if (root1 == root2) return;//За Крускал това може и да се пропусне

		if (rank[root1]>rank[root2]) parent[root2] = root1;      //Ако сливаме дървета с различен ранк, ранка на
		else if (rank[root2]>rank[root1]) parent[root1] = root2; //по-голямото не се променя
		else
		{
			parent[root2] = root1; //Ранка се увеличава
			rank[root1]++;       //Само когато сливаме дървета с еднакъв ранк
		}
		setCount--;
	}
};

struct Edge
{
	int src, dest, cost;
	int origId;
};

bool cmp(Edge e1, Edge e2)
{
	return e1.cost<e2.cost;
}

Edge edges[MAXM];
int treeEdges[MAXN];
int treeEdgeIndex = 0;
int treeCost = 0;

char idColor, idCost, idEdgeUsed; // свойствата ни

void InitProperties()
{
	idColor = pm.RegisterProperty(VERTEX_PROPERTY, "color", ColorRGB(255, 255, 255)); // цвят на връх
	idCost = pm.RegisterProperty(EDGE_PROPERTY | TYPE_COST, "cost", -1); // цена на ребро
	idEdgeUsed = pm.RegisterProperty(EDGE_PROPERTY | TYPE_MARKED, "used", false); // обходеност на ребро
}

#define NCOLORS 30
ColorRGB colors[NCOLORS]; // цветовете за компонентите

void MakeColors()
{ // създаваме цветовете
	srand(time(0));
	for (int i = 0; i < NCOLORS; i++)
	{
		colors[i].r = 50 + rand() % 206;
		colors[i].g = 50 + rand() % 206;
		colors[i].b = 50 + rand() % 206;
	}
}

DisjointSetForest dsf;

int cNodes, cEdges;

void UpdateVertexColors(int parent)
{ // обновяваме цветовете на всички от дадената компонента
	for (int i = 0; i < cNodes; i++)
	{
		if (dsf.find(i) == parent)
		{
			pm.SetProperty(i, idColor, colors[parent]);
		}
	}
}

int main()
{
	if (!Initialise()) return 0; // стандартен старт
	InitProperties();
	MakeColors();

	cin >> cNodes >> cEdges;
	SetVertices(cNodes); // задаваме брой върхове и ребра
	SetEdges(cEdges);

	dsf.create(cNodes);

	for (int i = 0; i < cEdges; i++)
	{
		cin >> edges[i].src >> edges[i].dest >> edges[i].cost;
		edges[i].origId = i; // пазим позицията в първоначалния ред, защото ще ни трябва после
		AddEdge(edges[i].src, edges[i].dest); // добавяме реброто
		pm.SetProperty(i, idCost, edges[i].cost); // и му задаваме цената
	}

	StartGui();  // пускаме визуалната среда

	Pause();

	sort(edges, edges + cEdges, cmp);

	for (int i = 0; i < cEdges; i++)
	{
		int r1 = dsf.find(edges[i].src);
		int r2 = dsf.find(edges[i].dest);
		if (r1 == r2) continue;

		treeEdges[treeEdgeIndex] = i;
		treeEdgeIndex++;
		treeCost += edges[i].cost;

		dsf.merge(r1, r2);
		//cout << "Merged " << r1 << " with " << r2 << endl;
		UpdateVertexColors(dsf.parent[r1]); // обновяваме цветовете на разширената вече компонента
		pm.SetProperty(edges[i].origId, idEdgeUsed, true); // маркираме реброто като обходено
		Pause(); // правим стъпка

		if (dsf.setCount == 1) break;
	}

	if (dsf.setCount != 1)
	{
		cout << "Graph is not connected!\n";
		cout << "Number of connected components: " << dsf.setCount << endl;
	}
	else
	{
		cout << "Tree cost: " << treeCost << endl;
		for (int i = 0; i < treeEdgeIndex; i++) cout << edges[treeEdges[i]].src << " " << edges[treeEdges[i]].dest << endl;
	}

	system("pause");

	return 0;
}
