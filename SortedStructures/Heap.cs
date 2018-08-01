using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SortedStructures
{
	//2k+1 2k+2 - left and right

	class Heap<T> : IEnumerable
		where T : IComparable<T>
	{
		T[] data;
		int size;
		public Heap(int size)
		{
			size = 0;
			data = new T[size];
		}

		public Heap() :this(100)
		{
		}

		public Heap(IEnumerable<T> l) : this(l.Count())
        {
			AddRange(l);
        }

		public void AddRange(IEnumerable<T> l)
		{
			foreach(T x in l)
                Insert(x);
		}
        
		public T ExtractMin()
		{
			if(size==0)
				throw new InvalidOperationException("Heap is empty");
			T result = data[0];
			Console.WriteLine(size);
			data[0] = data[size - 1];
			size--;
			HeapifyDown(0);
			return result;
		}

		public void Insert(T val)
		{
			if(size>= data.Length)
				IncreaseSize();
			data[size] = val;
			Heapify(size);
			size++;
		}

		private void IncreaseSize()
		{
			T[] newData = new T[2*data.Length+1];
			for(int i=0;i<data.Length;i++)
				newData[i] = data[i];
			data = newData;
		}

		private void Heapify(int i)
		{
			if (i > 0)
			{
				int parent = ParentLoc(i);
				if (data[parent].CompareTo(data[i]) > 0)
				{
					Swap(ref data[i], ref data[parent]);
					Heapify(parent);
				}
			}
		}

		private void Swap(ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}

		private void HeapifyDown(int i)
		{
			int leftChild = LeftChildLoc(i);
			int rightChild = RightChildLoc(i);
			int minChild = leftChild >= size ? -1 :
				rightChild >= size ? leftChild :
				(data[leftChild].CompareTo(data[rightChild]) > 0 ?
								rightChild : leftChild);

			if (minChild !=-1 && data[i].CompareTo(data[minChild]) > 0)
			{
				Swap(ref data[i], ref data[minChild]);
				HeapifyDown(minChild);
			}
		}

		private static int ParentLoc(int i) => (i - 1) / 2;
		private static int LeftChildLoc(int i) => 2 * i + 1;
		private static int RightChildLoc(int i) => 2 * i + 2;

		IEnumerator IEnumerable.GetEnumerator() => data.Take(size).GetEnumerator();
	}
}


