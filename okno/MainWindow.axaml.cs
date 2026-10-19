using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace okno;

public partial class MainWindow : Window
{
    private int _editingIndex = -1;

    public MainWindow()
    {
        InitializeComponent();
        DateTextBlock.Text = $"Data: {DateTime.Now:dd.MM.yyyy}";
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        AddOrSaveText();
    }

    private void OnInputKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddOrSaveText();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            CancelEdit();
            e.Handled = true;
        }
    }

    private void AddOrSaveText()
    {
        var text = InputBox.Text?.Trim();
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        if (_editingIndex >= 0 && _editingIndex < NotesList.Items.Count)
        {
            NotesList.Items[_editingIndex] = text;
            NotesList.SelectedIndex = _editingIndex;
        }
        else
        {
            NotesList.Items.Add(text);
            NotesList.SelectedIndex = NotesList.Items.Count - 1;
        }

        CancelEdit();
        InputBox.Focus();
    }

    private void OnEditClick(object? sender, RoutedEventArgs e)
    {
        StartEdit(NotesList.SelectedIndex);
    }

    private void OnEditItemClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            StartEdit(NotesList.Items.IndexOf(button.DataContext));
        }
    }

    private void OnNotesDoubleTapped(object? sender, TappedEventArgs e)
    {
        StartEdit(NotesList.SelectedIndex);
    }

    private void StartEdit(int index)
    {
        if (index < 0 || index >= NotesList.Items.Count)
        {
            return;
        }

        _editingIndex = index;
        NotesList.SelectedIndex = index;
        InputBox.Text = NotesList.Items[index]?.ToString();
        AddButton.Content = "Zapisz";
        InputBox.Focus();
        InputBox.SelectAll();
    }

    private void CancelEdit()
    {
        _editingIndex = -1;
        AddButton.Content = "Dodaj";
        InputBox.Clear();
    }

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        DeleteSelected();
    }

    private void OnDeleteItemClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is not null)
        {
            var index = NotesList.Items.IndexOf(button.DataContext);
            NotesList.Items.Remove(button.DataContext);
            if (_editingIndex == index)
            {
                CancelEdit();
            }
            else if (_editingIndex > index)
            {
                _editingIndex--;
            }
        }
    }

    private void OnNotesKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete)
        {
            DeleteSelected();
            e.Handled = true;
        }
        else if (e.Key == Key.F2)
        {
            StartEdit(NotesList.SelectedIndex);
            e.Handled = true;
        }
    }

    private void DeleteSelected()
    {
        var index = NotesList.SelectedIndex;
        if (index < 0)
        {
            return;
        }

        NotesList.Items.RemoveAt(index);
        if (_editingIndex == index)
        {
            CancelEdit();
        }
        else if (_editingIndex > index)
        {
            _editingIndex--;
        }
    }
}
